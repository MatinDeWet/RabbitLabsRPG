using MediatR;

namespace BuildingBlock.Domain.Abstractions.Contracts;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.CreateVersion7();
    public DateTimeOffset OccurredOn => DateTimeOffset.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;
}
