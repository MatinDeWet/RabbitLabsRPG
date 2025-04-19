using BuildingBlock.Domain.Abstractions;
using BuildingBlock.Domain.Abstractions.Contracts;
using Shouldly;

namespace BuildingBlock.Domain.Tests.Tests.Abstractions;

public class AggregateTests
{
    private sealed class TestAggregate : Aggregate<Guid>
    {
        public TestAggregate()
        {
            // The constructor now just sets the DateCreated, not Id
            DateCreated = DateTimeOffset.UtcNow;
        }
    }

    private sealed class TestDomainEvent : IDomainEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public DateTimeOffset OccurredOn { get; set; } = DateTimeOffset.UtcNow;
        public string EventType { get; set; } = "TestEvent";
    }

    [Fact]
    public void DomainEvents_ShouldBeEmptyOnCreation()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer

        // Assert
        aggregate.DomainEvents.ShouldNotBeNull();
        aggregate.DomainEvents.Count.ShouldBe(0);
    }

    [Fact]
    public void AddDomainEvent_ShouldAddEventToDomainEvents()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer
        var domainEvent = new TestDomainEvent();

        // Act
        aggregate.AddDomainEvent(domainEvent);

        // Assert
        aggregate.DomainEvents.ShouldNotBeNull();
        aggregate.DomainEvents.Count.ShouldBe(1);
        aggregate.DomainEvents[0].ShouldBe(domainEvent); // Use indexer instead of First()
    }

    [Fact]
    public void AddDomainEvent_ShouldAllowAddingMultipleEvents()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer
        var domainEvent1 = new TestDomainEvent();
        var domainEvent2 = new TestDomainEvent();
        var domainEvent3 = new TestDomainEvent();

        // Act
        aggregate.AddDomainEvent(domainEvent1);
        aggregate.AddDomainEvent(domainEvent2);
        aggregate.AddDomainEvent(domainEvent3);

        // Assert
        aggregate.DomainEvents.ShouldNotBeNull();
        aggregate.DomainEvents.Count.ShouldBe(3);
        aggregate.DomainEvents.ShouldContain(domainEvent1);
        aggregate.DomainEvents.ShouldContain(domainEvent2);
        aggregate.DomainEvents.ShouldContain(domainEvent3);
    }

    [Fact]
    public void ClearDomainEvents_ShouldReturnAllEventsAndClearCollection()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer
        var domainEvent1 = new TestDomainEvent();
        var domainEvent2 = new TestDomainEvent();
        aggregate.AddDomainEvent(domainEvent1);
        aggregate.AddDomainEvent(domainEvent2);

        // Act
        IDomainEvent[] events = aggregate.ClearDomainEvents();

        // Assert
        events.ShouldNotBeNull();
        events.Length.ShouldBe(2);
        events.ShouldContain(domainEvent1);
        events.ShouldContain(domainEvent2);

        // Verify the events were cleared
        aggregate.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void ClearDomainEvents_ShouldReturnEmptyArray_WhenNoEvents()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer

        // Act
        IDomainEvent[] events = aggregate.ClearDomainEvents();

        // Assert
        events.ShouldNotBeNull();
        events.Length.ShouldBe(0);
        aggregate.DomainEvents.ShouldBeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldBeReadOnly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer
        var domainEvent = new TestDomainEvent();
        aggregate.AddDomainEvent(domainEvent);

        // Assert
        aggregate.DomainEvents.ShouldBeAssignableTo<IReadOnlyList<IDomainEvent>>();
        Should.Throw<NotSupportedException>(() =>
        {
            // Attempt to modify the collection (which should fail)
            var domainEvents = (IList<IDomainEvent>)aggregate.DomainEvents;
            domainEvents.Add(new TestDomainEvent());
        });
    }

    [Fact]
    public void AddDomainEvent_ShouldThrowArgumentNullException_WhenEventIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var aggregate = new TestAggregate() { Id = id }; // Set Id in object initializer

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => aggregate.AddDomainEvent(null!)); // Add ! to handle non-nullable reference
    }
}
