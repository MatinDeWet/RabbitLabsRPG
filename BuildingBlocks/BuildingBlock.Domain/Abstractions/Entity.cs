using BuildingBlock.Domain.Abstractions.Contracts;

namespace BuildingBlock.Domain.Abstractions;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public required TKey Id { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
}
