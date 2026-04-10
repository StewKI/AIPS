using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Dispatcher;
using AipsCore.Infrastructure.MessageBroking.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class AipsRegistrationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAips(IConfiguration configuration)
        {
            services.AddAipsHandlersFromAssembly(typeof(Dispatcher).Assembly);
            services.AddAipsDispatcher();

            services.AddAipsPersistence(configuration);

            services.AddAipsAuth(configuration);
            services.AddAipsUserContext();

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
            services.AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();
        
            return services;
        }

        public IServiceCollection AddAipsMessageHandlers()
        {
            services.AddAipsMessageHandlersFromAssembly(typeof(IMessage).Assembly);

            return services;
        }
    }
}