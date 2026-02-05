using AipsCore.Domain.Abstract;

namespace AipsCore.Infrastructure.Db;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AipsDbContext _dbContext;

    public EfUnitOfWork(AipsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}