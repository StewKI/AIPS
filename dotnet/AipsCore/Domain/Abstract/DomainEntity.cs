using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Abstract;

public abstract class DomainEntity<TId> where TId : DomainId
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public TId Id { get; init; }
    
    protected DomainEntity()
    {
        
    }

    protected DomainEntity(TId id)
    {
        Id = id;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}