using AipsCore.Infrastructure.DI.Configuration;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class PersistenceRegistrationExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AipsDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetEnvConnectionString(), npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(AipsDbContext).Assembly.FullName);
            });
        });
        
        return services;
    }
}