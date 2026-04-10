using AipsCore.Domain.Models.Shape.External;
using AipsCore.Domain.Models.User.External;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.WhiteboardMembership.External;
using AipsCore.Infrastructure.Persistence.Shape;
using AipsCore.Infrastructure.Persistence.User;
using AipsCore.Infrastructure.Persistence.Whiteboard;
using AipsCore.Infrastructure.Persistence.WhiteboardMembership;
using Microsoft.Extensions.DependencyInjection;

namespace AipsCore.Infrastructure.DI;

public static class RepositoryRegistrationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAipsRepositories()
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWhiteboardRepository, WhiteboardRepository>();
            services.AddScoped<IWhiteboardMembershipRepository, WhiteboardMembershipRepository>();
            services.AddScoped<IShapeRepository, ShapeRepository>();
            
            return services;
        }
    }
}