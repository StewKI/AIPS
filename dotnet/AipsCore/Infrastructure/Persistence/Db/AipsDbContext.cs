using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Db;

public class AipsDbContext : DbContext
{
    public DbSet<User.User> Users { get; set; }

    public AipsDbContext(DbContextOptions options)
        : base(options)
    {
        
    }
}