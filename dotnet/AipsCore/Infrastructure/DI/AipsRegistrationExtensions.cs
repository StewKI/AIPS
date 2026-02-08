using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Common.Dispatcher;
using AipsCore.Application.Models.User.Command.CreateUser;
using AipsCore.Domain.Models.User.ValueObjects;
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
        
        return services;
    }
}