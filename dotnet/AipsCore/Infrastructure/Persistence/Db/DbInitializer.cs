using AipsCore.Domain.Models.User.External;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.Persistence.Db;

public static class DbInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var roleNames = UserRole.All();

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName.Name);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = roleName.Name,
                    NormalizedName = roleName.Name.ToUpperInvariant()
                });
            }
        }
    }
}