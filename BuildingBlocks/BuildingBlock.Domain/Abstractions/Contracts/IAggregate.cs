namespace BuildingBlock.Domain.Abstractions.Contracts;

public interface IAggregate<T> : IAggregate, IEntity<T>;

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}
