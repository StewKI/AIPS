using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.Persistence.Db;

public static class StartupExtensions
{
    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        await DbInitializer.SeedRolesAsync(serviceProvider);
    }
}