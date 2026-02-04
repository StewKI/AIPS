using AipsCore.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Db;

public class AipsDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}