using AipsCore.Application.Abstract.UserContext;
using AipsCore.Domain.Abstract;
using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Infrastructure.DI.Configuration;
using AipsCore.Infrastructure.Persistence.Db;
using AipsCore.Infrastructure.Persistence.RefreshToken;
using AipsCore.Infrastructure.Persistence.Shape;
using AipsCore.Infrastructure.Persistence.User;
using AipsCore.Infrastructure.Persistence.Whiteboard;
using AipsCore.Infrastructure.Persistence.WhiteboardMembership;
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

        services.AddTransient<IUnitOfWork, EfUnitOfWork>();
        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IWhiteboardRepository, WhiteboardRepository>();
        services.AddTransient<IWhiteboardMembershipRepository, WhiteboardMembershipRepository>();
        services.AddTransient<IShapeRepository, ShapeRepository>();

        services.AddTransient<IRefreshTokenManager, RefreshTokenManager>();
        
        return services;
    }
}