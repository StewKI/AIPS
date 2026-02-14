using System.Reflection;
using AipsCore.Application.Abstract.MessageBroking;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class MessageHandlerRegistrationExtensions
{
    public static IServiceCollection AddMessageHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterface = typeof(IMessageHandler<>);
        
        var types = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false });

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericDef = @interface.GetGenericTypeDefinition();

                if (handlerInterface != genericDef)
                    continue;
                
                services.AddTransient(@interface, type);
            }
        }

        return services;
    }
}