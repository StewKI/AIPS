using AipsCore.Application.Abstract;
using AipsCore.Application.Common.Dispatcher;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class DispatcherRegistrationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAipsDispatcher()
        {
            services.AddTransient<IDispatcher, Dispatcher>();
            
            return services;
        }
    }
}