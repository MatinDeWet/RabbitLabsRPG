using BuildingBlock.Domain.Abstractions.Contracts;

namespace BuildingBlock.Domain.Abstractions;

public abstract class Aggregate<TKey> : Entity<TKey>, IAggregate<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeuedEvents = [.. _domainEvents];

        _domainEvents.Clear();

        return dequeuedEvents;
    }
}
