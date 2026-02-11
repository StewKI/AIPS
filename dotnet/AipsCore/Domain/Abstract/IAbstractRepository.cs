using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Abstract;

public interface IAbstractRepository<TEntity, in TId>
    where TEntity : DomainEntity<TId>
    where TId : DomainId
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}