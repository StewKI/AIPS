using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Infrastructure.DI.Configuration;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.RefreshToken;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class PersistenceRegistrationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAipsPersistence(IConfiguration configuration)
        {
            services.AddDbContext<AipsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetEnvConnectionString(), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(AipsDbContext).Assembly.FullName);
                });
            });

            services.AddTransient<IUnitOfWork, EfUnitOfWork>();

            services.AddAipsRepositories();

            services.AddTransient<IRefreshTokenManager, RefreshTokenManager>();
        
            return services;
        }
    }
}