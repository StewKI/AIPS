using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Abstract;

public interface ISoftDeletableRepository<in TId> where TId : DomainId
{
    Task SoftDeleteAsync(TId id, CancellationToken cancellationToken = default);
}