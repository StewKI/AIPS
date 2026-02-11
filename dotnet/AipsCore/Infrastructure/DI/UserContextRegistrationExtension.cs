using AipsCore.Application.Abstract.UserContext;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class UserContextRegistrationExtension
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddTransient<IUserContext, UserContext>();
        
        return services;
    }
}