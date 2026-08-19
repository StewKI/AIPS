# AIPS (Advanced Interactive Painting System)

A real-time collaborative whiteboard. One user creates a board and shares it, and several participants then draw on it at the same time, with every change showing up for everyone right away. The available shapes are rectangle, line, arrow and text. The board owner controls who gets in: depending on the join policy a participant either enters directly or waits to be approved, and the owner can also reject, kick or ban. Authentication is JWT with refresh tokens.

<img src="docs/screenshots/screenshot_3.png" alt="A whiteboard in session with two participants" width="900">

A board in session. Everyone present is listed in the sidebar, every shape carries the name of whoever drew it, and the eight digit code under the list is what other people use to get in.

<table>
<tr>
<td width="50%"><img src="docs/screenshots/screenshot_1.png" alt="Home screen" width="100%"></td>
<td width="50%"><img src="docs/screenshots/screenshot_2.png" alt="Creating a new whiteboard" width="100%"></td>
</tr>
<tr>
<td>Joining an existing board by code, or starting one of your own.</td>
<td>A new board takes a title, a join policy and a participant limit.</td>
</tr>
</table>

## Architecture

<img src="docs/diagrams/communication-components.png" alt="AIPS components and communication paths" width="872">

Nginx is the single entrypoint. Behind it the REST path and the realtime path go their separate ways, and they only meet again at the database.

### What happens when you draw a shape

There are two paths through the system, a fast one for realtime and a durable one for persistence, and they are only loosely coupled:

1. The client draws and invokes a hub method over **SignalR** (`AddRectangle`, `MoveShape` and so on).
2. **AipsRT** (the realtime service) changes the board in its **in-memory** state and immediately broadcasts that change to the other participants on the board.
3. At the same time, RT publishes a message (`AddRectangleMessage`, `MoveShapeMessage`) to a **RabbitMQ** topic exchange.
4. **AipsWorker** picks the message up and turns it into a command. The domain validates its rules and the result is written to **PostgreSQL**.
5. If validation fails, the Worker publishes an `ErrorMessage`. RT receives it, reloads the board from the database and sends `InitWhiteboard` to everyone, so the in-memory state goes back to whatever was actually saved.

So drawing feels instant, because nothing waits on the database, but the database is still the source of truth and it corrects memory whenever the two drift apart.

Everything that isn't drawing (signup, login, creating and deleting boards, history, joining by code) goes the ordinary REST way through **AipsWebApi**.

## Components

One row per box on the diagram, in the same order:

| Box on the diagram | Code / config | Role |
|---|---|---|
| **Vue client** | `front/`, built with Vue 3, TypeScript, Vite and Pinia | SPA. The canvas is SVG, and it talks to the backend over `fetch` for REST and `@microsoft/signalr` for the hub |
| **Nginx** | `deploy/nginx/` | The single entrypoint. `/api/` goes to AipsWebApi, `/hubs/` goes to AipsRT with a WebSocket upgrade, everything else is served as the static SPA build. Only used in deployment, since Vite proxies during development |
| **AipsRT** | `dotnet/AipsRT/`, ASP.NET Core with SignalR | Serves the realtime hub at `/hubs/whiteboard`. Keeps active boards in memory (`WhiteboardManager`), broadcasts changes to the board's group, publishes messages and listens for `ErrorMessage` |
| **AipsWebApi** | `dotnet/AipsWebApi/`, ASP.NET Core Web API | REST endpoints: `/api/User/*` for signup, login, refresh, logout and me, `/api/Whiteboard/*` for create, get, delete, history, recent and join |
| **RabbitMQ** | `rabbitmq:3-management` via Docker | Topic exchange. Both the routing key and the queue name are the message type name, and messages are acknowledged manually |
| **AipsWorker** | `dotnet/AipsWorker/`, .NET Worker Service | Subscribes to the drawing and membership messages, runs them as commands, saves the result, and publishes `ErrorMessage` if validation fails |
| **DB** | `postgres:18` via Docker | Users, whiteboards, shapes, whiteboard memberships and refresh tokens |

`dotnet/AipsCore` has no box on the diagram because it isn't a process. It is the class library the three .NET services are built on, and most of the design sits in it:

- `Domain` for models, value objects and validation rules
- `Application` for commands, queries, messages, their handlers and the dispatcher that routes them
- `Infrastructure` for EF Core and migrations, the RabbitMQ publisher and subscriber, JWT and the DI wiring

That shared library is the reason WebApi, RT and Worker can all work on the same model. All three host the same dispatcher and the same handlers, and the only difference between them is what sets a handler off: an HTTP request, a SignalR call, or a message from the broker.

## Where to look in the code

The frontend is a thin client that draws and relays. Almost all of the design sits on the .NET side, so this is the order that makes sense for reading it.

**What starts the work**

- `dotnet/AipsRT/Hubs/WhiteboardHub.cs` is the entire realtime surface. Every drawing action lands here, updates the in-memory board, publishes a message and broadcasts to the group.
- `dotnet/AipsWorker/WorkerService.cs` subscribes to every message type, runs each one as a command, and converts a `ValidationException` into an `ErrorMessage` that goes back out through the broker.
- `dotnet/AipsWebApi/Controllers/` are deliberately thin. A controller builds a command or a query, hands it to the dispatcher and returns the result.

**The shared core, in `dotnet/AipsCore`**

- `Application/Common/Dispatcher/Dispatcher.cs` resolves the handler from the command or query type and invokes it. This is why a hub call, an HTTP request and a broker message all end up executing the same way.
- `Application/Abstract/` holds the interfaces everything else is written against: `ICommand`, `ICommandHandler`, `IQuery`, `IMessagePublisher`, `IMessageSubscriber`.
- `Application/Models/Shape/Command/CreateRectangle/` is one command and handler pair, if you want a single vertical slice to read.
- `Domain/Abstract/ValueObject/AbstractValueObject.cs` and `Domain/Common/Validation/` are the validation approach. A value object declares its rules, the validator runs all of them and collects every error, and construction then fails with a `ValidationException` that carries the whole list instead of only the first problem.
- `Domain/Models/Shape/` is a domain model built out of those value objects, with factory methods and no persistence concerns in it. There is no EF reference anywhere under `Domain/`.

**Infrastructure**

- `Infrastructure/Persistence/Abstract/AbstractRepository.cs` and `Infrastructure/Persistence/Shape/Mappers/` keep the EF entities separate from the domain models, with the mapping written out explicitly in both directions.
- `Infrastructure/MessageBroking/RabbitMQ/` is the only code that knows RabbitMQ exists. Changing the broker means writing another pair of these classes and nothing else.
- `dotnet/AipsWorker/Utilities/SubscribeMethodUtility.cs` binds the generic `SubscribeAsync<T>` for each message type through reflection, which is how the Worker registers all of its subscriptions from a plain list of types.
- `dotnet/AipsRT/Services/RtErrorHandleStrategy.cs` is what actually runs when persistence rejects a change: reload the board from the database and re-initialise every connected client.

## Requirements

| What | Version | Notes |
|---|---|---|
| .NET SDK | 10.0 | All three services target `net10.0` |
| `dotnet-ef` | 10.x | Needed once to create the schema, install with `dotnet tool install --global dotnet-ef` |
| Docker | Engine with Compose v2 | Runs the two infrastructure containers |
| Bun | 1.x | Installs the frontend and runs the dev server. Node 20.19+ or 22.12+ with npm works as well |

Postgres 18 and RabbitMQ 3 with the management plugin are pulled as images by `start-infra.sh`, so there is nothing to install for either. On the .NET side the project is on EF Core 10, Npgsql 10 and RabbitMQ.Client 7.

## Running locally

```bash
cp deploy/.env.example .env   # fill in the values, and see the note below
./start-infra.sh              # Postgres :5432, RabbitMQ :5672 / UI :15672
./start-back.sh               # WebApi :5266, RT :5039, Worker
./start-front.sh              # Vite :5173 (proxies /api and /hubs to the backend)
```

`deploy/.env.example` lists the variables the Docker deployment needs, where the compose file assembles the connection strings itself. Running the services straight on the host means they read those two strings directly, so add them to `.env` as well:

```
DB_CONN_STRING=Host=localhost;Port=5432;Database=aips_db;Username=aips_user;Password=<your password>
RABBITMQ_AMQP_URI=amqp://<user>:<password>@localhost:5672/%2F
```

The schema is not created on startup, so apply the migrations once before the first run:

```bash
cd dotnet && dotnet ef database update --project AipsCore --startup-project AipsWebApi
```

What WebApi does do on startup is seed the Identity roles, in `InitializeInfrastructureAsync`.

For production, `deploy/docker-compose.yml` builds every service and exposes nginx on `:8090`, with `deploy/nginx/aips-global.conf` acting as the TLS proxy on the host.

## Documentation

`docs/` holds the project documents for each phase, covering architecture, the data and persistence model, and the communication models. Diagram sources (drawio) and their PNG exports are in `docs/diagrams/`, and the screenshots above are in `docs/screenshots/`. The phase documents are written in Serbian.

## Authors

Built by [Andrija Stevanović](https://github.com/StewKI) and [Veljko Tošić](https://github.com/veljkotosic) as a university project for the Software Architecture and Design course (Arhitektura i projektovanje softvera). We worked on it together throughout, so there is no split of the codebase between us.
