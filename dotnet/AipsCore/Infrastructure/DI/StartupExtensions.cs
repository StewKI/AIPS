using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class StartupExtensions
{
    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        await DbInitializer.SeedRolesAsync(serviceProvider);
    }
}