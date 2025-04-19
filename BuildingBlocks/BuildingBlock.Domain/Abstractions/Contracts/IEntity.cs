namespace BuildingBlock.Domain.Abstractions.Contracts;

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}

public interface IEntity
{
    public DateTimeOffset DateCreated { get; set; }
}
