using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Abstract;

public abstract class AbstractRepository<TEntity, TId, TPersistenceEntity> : IAbstractRepository<TEntity, TId>
    where TEntity : DomainEntity<TId>
    where TId : DomainId
    where TPersistenceEntity : class
{
    protected readonly AipsDbContext Context;
    protected readonly DbSet<TPersistenceEntity> DbSet;
    
    protected AbstractRepository(AipsDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TPersistenceEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var persistenceEntity = await DbSet.FindAsync([new Guid(id.IdValue)], cancellationToken);
        
        return persistenceEntity != null ? MapToDomainEntity(persistenceEntity) : null;
    }

    public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var persistenceEntity = await DbSet.FindAsync([new Guid(entity.Id.IdValue)], cancellationToken);

        if (persistenceEntity == null)
        {
            persistenceEntity = MapToPersistenceEntity(entity);
            await DbSet.AddAsync(persistenceEntity, cancellationToken);
        }
        else
        {
            UpdatePersistenceEntity(persistenceEntity, entity);
            DbSet.Update(persistenceEntity);
        }
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var persistenceEntity = MapToPersistenceEntity(entity);
        
        await DbSet.AddAsync(persistenceEntity, cancellationToken);
    }

    protected abstract TEntity MapToDomainEntity(TPersistenceEntity persistenceEntity);
    protected abstract TPersistenceEntity MapToPersistenceEntity(TEntity domainEntity);
    protected abstract void UpdatePersistenceEntity(TPersistenceEntity persistenceEntity, TEntity domainEntity);
}