using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Dispatcher;
using AipsCore.Infrastructure.MessageBroking.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class AipsRegistrationExtensions
{
    public static IServiceCollection AddAips(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHandlersFromAssembly(typeof(Dispatcher).Assembly);
        services.AddTransient<IDispatcher, Dispatcher>();

        services.AddPersistence(configuration);

        services.AddUserContext(configuration);

        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();
        
        return services;
    }

    public static IServiceCollection AddAipsMessageHandlers(this IServiceCollection services)
    {
        services.AddMessageHandlersFromAssembly(typeof(IMessage).Assembly);

        return services;
    }
}