using Bogus;
using BuildingBlock.Application.Pagination;
using BuildingBlock.Application.Pagination.Enums;
using BuildingBlock.Application.Pagination.Models;
using MockQueryable;
using Shouldly;

namespace BuildingBlock.Application.Tests.Tests.Pagination;

public class PageableExtensionsTests
{
    private readonly Faker<TestEntity> _faker;

    public PageableExtensionsTests()
    {
        _faker = new Faker<TestEntity>()
            .RuleFor(e => e.Id, f => f.IndexFaker)
            .RuleFor(e => e.Name, f => f.Name.FirstName());
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldReturnPagedResponse()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker
            .Generate(50)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 2,
            PageSize = 10,
            OrderBy = "Name",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .OrderBy(e => e.Name)
            .ToPageableListAsync(request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(10);
        result.PageNumber.ShouldBe(2);
        result.PageSize.ShouldBe(10);
        result.TotalRecords.ShouldBe(50);
        result.PageCount.ShouldBe(5);
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldThrowArgumentOutOfRangeException_WhenPageNumberIsInvalid()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(10)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 0, // Invalid
            PageSize = 10
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act & Assert
        await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await data.ToPageableListAsync(request, cancellationToken);
        });
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldOrderByDescending_WhenOrderDirectionIsDescending()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(20)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 5,
            OrderBy = "Name",
            OrderDirection = OrderDirectionEnum.Descending
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .OrderByDescending(e => e.Name)
            .ToPageableListAsync(request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(5);
        result.OrderDirection.ShouldBe(OrderDirectionEnum.Descending);
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldThrowArgumentOutOfRangeException_WhenPageSizeIsInvalid()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(10)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 0 // Invalid
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act & Assert
        await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await data.ToPageableListAsync(request, cancellationToken);
        });
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldReturnEmptyResponse_WhenDataIsEmpty()
    {
        // Arrange
        IQueryable<TestEntity> data = Enumerable.Empty<TestEntity>()
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "Name",
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data.ToPageableListAsync(request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        result.TotalRecords.ShouldBe(0);
        result.PageCount.ShouldBe(0);
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldHandleLargePageNumber_Gracefully()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(10)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 100, // Large page number
            PageSize = 10,
            OrderBy = "Name",
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data.ToPageableListAsync(request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        result.TotalRecords.ShouldBe(10);
        result.PageCount.ShouldBe(1);
    }

    [Fact]
    public async Task ToPageableListAsync_ShouldOrderByAscending_WhenOrderDirectionIsAscending()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(20)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 5,
            OrderBy = "Name",
            OrderDirection = OrderDirectionEnum.Ascending
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .OrderBy(e => e.Name)
            .ToPageableListAsync(request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(5);
        result.OrderDirection.ShouldBe(OrderDirectionEnum.Ascending);
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderKeySelector_ShouldReturnPagedResponse()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(50)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 2,
            PageSize = 10
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .ToPageableListAsync(e => e.Name, request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(10);
        result.PageNumber.ShouldBe(2);
        result.PageSize.ShouldBe(10);
        result.TotalRecords.ShouldBe(50);
        result.PageCount.ShouldBe(5);
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderKeySelector_ShouldThrowArgumentOutOfRangeException_WhenPageNumberIsInvalid()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(10)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 0, // Invalid
            PageSize = 10
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act & Assert
        await Should.ThrowAsync<ArgumentOutOfRangeException>(async () =>
        {
            await data.ToPageableListAsync(e => e.Name, request, cancellationToken);
        });
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderKeySelector_ShouldThrowArgumentOutOfRangeException_WhenPageSizeIsInvalid()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(10)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 0 // Invalid
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act & Assert
        await Should.ThrowAsync<ArgumentOutOfRangeException>(async () =>
        {
            await data.ToPageableListAsync(e => e.Name, request, cancellationToken);
        });
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderKeySelector_ShouldOrderByAscending_WhenOrderDirectionIsAscending()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(20)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 5,
            OrderDirection = OrderDirectionEnum.Ascending
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .ToPageableListAsync(e => e.Name, request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(5);
        result.OrderDirection.ShouldBe(OrderDirectionEnum.Ascending);
    }

    [Fact]
    public async Task ToPageableListAsync_WithOrderKeySelector_ShouldOrderByDescending_WhenOrderDirectionIsDescending()
    {
        // Arrange
        IQueryable<TestEntity> data = _faker.Generate(20)
            .AsQueryable()
            .BuildMock();

        var request = new PagableTestEntity
        {
            PageNumber = 1,
            PageSize = 5,
            OrderDirection = OrderDirectionEnum.Descending
        };

        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        PageableResponse<TestEntity> result = await data
            .ToPageableListAsync(e => e.Name, request, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count().ShouldBe(5);
        result.OrderDirection.ShouldBe(OrderDirectionEnum.Descending);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by Faker for testing purposes.")]
internal sealed class TestEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

internal sealed class PagableTestEntity : PageableRequest
{
}
