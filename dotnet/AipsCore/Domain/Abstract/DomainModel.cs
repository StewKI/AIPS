using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Abstract;

public abstract class DomainModel<TId> where TId : DomainId
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public TId Id { get; init; }
    
    protected DomainModel()
    {
        
    }

    protected DomainModel(TId id)
    {
        Id = id;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}