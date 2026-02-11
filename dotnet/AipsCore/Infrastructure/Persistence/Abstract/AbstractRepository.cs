using AipsCore.Domain.Abstract;
using AipsCore.Domain.Common.ValueObjects;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace AipsCore.Infrastructure.Persistence.Abstract;

public abstract class AbstractRepository<TModel, TId, TEntity> : IAbstractRepository<TModel, TId>
    where TModel : DomainModel<TId>
    where TId : DomainId
    where TEntity : class
{
    protected readonly AipsDbContext Context;
    protected readonly DbSet<TEntity> DbSet;
    
    protected AbstractRepository(AipsDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }
    
    public async Task<TModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FindAsync([new Guid(id.IdValue)], cancellationToken);
        
        return entity != null ? MapToModel(entity) : null;
    }

    public async Task SaveAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FindAsync([new Guid(model.Id.IdValue)], cancellationToken);

        if (entity == null)
        {
            entity = MapToEntity(model);
            await DbSet.AddAsync(entity, cancellationToken);
        }
        else
        {
            UpdateEntity(entity, model);
            DbSet.Update(entity);
        }
    }

    public async Task AddAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(model);
        
        await DbSet.AddAsync(entity, cancellationToken);
    }

    protected abstract TModel MapToModel(TEntity entity);
    protected abstract TEntity MapToEntity(TModel model);
    protected abstract void UpdateEntity(TEntity entity, TModel model);
}