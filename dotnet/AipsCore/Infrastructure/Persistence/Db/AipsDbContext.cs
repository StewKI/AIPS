using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Db;

public class AipsDbContext : IdentityDbContext<User.User, IdentityRole<Guid>, Guid>
{
    public DbSet<RefreshToken.RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<Whiteboard.Whiteboard> Whiteboards { get; set; }
    public DbSet<Shape.Shape> Shapes { get; set; }
    public DbSet<WhiteboardMembership.WhiteboardMembership> WhiteboardMemberships { get; set; }

    public AipsDbContext(DbContextOptions<AipsDbContext> options)
        : base(options)
    {
        
    }
}