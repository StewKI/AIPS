using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Abstract;

public interface IAbstractRepository<TModel, in TId>
    where TModel : DomainModel<TId>
    where TId : DomainId
{
    Task<TModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task SaveAsync(TModel model, CancellationToken cancellationToken = default);
    Task AddAsync(TModel model, CancellationToken cancellationToken = default);
}