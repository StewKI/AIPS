using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Db;

public class AipsDbContext : DbContext
{
    public DbSet<User.User> Users { get; set; }
    public DbSet<Whiteboard.Whiteboard> Whiteboards { get; set; }
    public DbSet<Shape.Shape> Shapes { get; set; }

    public AipsDbContext(DbContextOptions<AipsDbContext> options)
        : base(options)
    {
        
    }
}