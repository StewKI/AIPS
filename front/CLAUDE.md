# AIPS Frontend

Vue 3 SPA with Bootstrap 5 dark theme, authentication UI, and a service layer ready for backend integration.

## Tech Stack
- **Vue 3** + **TypeScript** + **Vite** — Composition API (`<script setup>`) everywhere
- **Pinia** — State management (composition API style stores)
- **Vue Router** — Client-side routing with navigation guards
- **Bootstrap 5** — Sass-only (no Bootstrap JS), custom dark theme via variable overrides
- **sass-embedded** — Compiles Bootstrap Sass + custom overrides

## Project Structure
```
src/
├── main.ts                     # Entry point — mounts app, installs Pinia, initializes auth
├── App.vue                     # Root: AppTopBar + <RouterView> in Bootstrap container
├── router/
│   └── index.ts                # Routes + beforeEach guards (guestOnly / requiresAuth)
├── stores/
│   └── auth.ts                 # Auth state: user, token, login/signup/logout (mock for now)
├── types/
│   └── index.ts                # Shared TS interfaces (User, LoginCredentials, etc.)
├── services/
│   ├── api.ts                  # Generic fetch wrapper — auto-attaches Bearer token
│   └── authService.ts          # Auth API calls (stub — throws "Not implemented")
├── components/
│   └── AppTopBar.vue           # Navbar: nav links + auth-aware right side
├── views/
│   ├── HomeView.vue            # "/" — placeholder
│   ├── AboutView.vue           # "/about" — placeholder (lazy-loaded)
│   ├── LoginView.vue           # "/login" — email/password form (lazy-loaded)
│   └── SignupView.vue          # "/signup" — username/email/password form (lazy-loaded)
└── assets/scss/
    ├── main.scss               # Imports: _variables → bootstrap → _app
    ├── _variables.scss          # Bootstrap variable overrides (dark theme colors)
    └── _app.scss               # App-level global styles
```

## Routing

| Path | Name | Guard | Component |
|------|------|-------|-----------|
| `/` | home | — | HomeView |
| `/about` | about | — | AboutView (lazy) |
| `/login` | login | `guestOnly` | LoginView (lazy) |
| `/signup` | signup | `guestOnly` | SignupView (lazy) |

- `guestOnly` — redirects authenticated users to `/`
- `requiresAuth` — redirects unauthenticated users to `/login` (no routes use this yet)

## Auth Flow (Currently Mocked)
Login and signup set a fake user/token and persist the token to `localStorage`. The service layer (`authService.ts`) has stubs that throw `"Not implemented"` — replace these with real API calls when the backend is ready.

Key files to wire up:
1. `src/services/authService.ts` — uncomment `api.post(...)` calls
2. `src/stores/auth.ts` — replace mock responses with `authService` calls
3. `src/services/api.ts` — `BASE_URL` reads from `VITE_API_URL` env var (defaults to `/api`)

## Styling
- All styling uses Bootstrap 5 utility classes in templates + Sass variable overrides
- Dark theme: `$body-bg: #121212`, `$card-bg: #1E1E1E`, `$primary: #4F9DFF`
- Custom variables go in `_variables.scss` (before Bootstrap import)
- App-specific global styles go in `_app.scss` (after Bootstrap import)
- Prefer Bootstrap utility classes over custom CSS

## Conventions
- **Composition API** with `<script setup lang="ts">` — no Options API
- **Pinia stores** use composition API style (`defineStore('name', () => { ... })`)
- Types live in `src/types/index.ts`
- API service functions live in `src/services/`
- Views are page-level components in `src/views/`, reusable components in `src/components/`
- New routes that need auth should use `meta: { requiresAuth: true }`

## Scripts
- `bun dev` — Start dev server
- `bun build` — Type-check (`vue-tsc`) + production build
- `bun lint` — Oxlint + ESLint

## Environment Variables
- `VITE_API_URL` — Backend API base URL (default: `/api`)
