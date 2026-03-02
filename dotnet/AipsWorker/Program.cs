using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Infrastructure.DI;
using AipsWorker;
using AipsWorker.Messages;
using AipsWorker.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

LoadingDotEnv.TryLoad();

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddAips(context.Configuration);
    services.AddAipsMessageHandlers();

    services.AddSingleton<IMessageTypesProvider, MessageTypesProvider>();

    services.AddHostedService<WorkerService>();
});

var app = builder.Build();
    
await app.RunAsync();