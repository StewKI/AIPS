using AipsCore.Application.Abstract.UserContext;
using AipsCore.Infrastructure.Authentication.UserContext;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class UserContextRegistrationExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAipsUserContext()
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, HttpUserContext>();
        
            return services;
        }
    }
}