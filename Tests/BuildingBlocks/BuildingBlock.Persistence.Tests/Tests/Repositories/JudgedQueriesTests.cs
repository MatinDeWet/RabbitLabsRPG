using BuildingBlock.Application.Security.Contracts;
using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories;
using BuildingBlock.Persistence.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace BuildingBlock.Persistence.Tests.Tests.Repositories;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
"Design",
"CA1515:Because an application's API isn't typically referenced from outside the assembly, types can be made internal",
Justification = "This class is intentionally public for testing purposes.")]
public sealed class JudgedQueriesDummyEntity { }

public class JudgedQueriesTests
{
    private static Mock<DbContext> CreateDbContextMock<T>(IQueryable<T> data) where T : class
    {
        var dbSetMock = new Mock<DbSet<T>>();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var dbContextMock = new Mock<DbContext>(new DbContextOptionsBuilder<DbContext>().Options);
        dbContextMock.Setup(c => c.Set<T>()).Returns(dbSetMock.Object);

        return dbContextMock;
    }

    [Fact]
    public void Secure_WhenUserIsSuperAdmin_ShouldReturnUnfilteredDbSet()
    {
        // Arrange
        var data = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity(),
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var dbContextMock = CreateDbContextMock(data);
        var identityMock = new Mock<IIdentityInfo>();

        // Setup SuperAdmin role
        identityMock.Setup(x => x.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        identityMock.Setup(x => x.GetIdentityId()).Returns(999);

        var requirements = new AccessRequirements();
        var protections = new List<IProtected>();

        var judgedQueries = new JudgedQueries<DbContext>(
            dbContextMock.Object,
            identityMock.Object,
            requirements,
            protections);

        // Act
        var result = judgedQueries.Secure<JudgedQueriesDummyEntity>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(dbContextMock.Object.Set<JudgedQueriesDummyEntity>());
        identityMock.Verify(x => x.HasRole(ApplicationRoleEnum.SuperAdmin), Times.Once);
    }

    [Fact]
    public void Secure_WhenEntityHasProtection_ShouldReturnSecuredQueryable()
    {
        // Arrange
        var data = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity(),
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var dbContextMock = CreateDbContextMock(data);
        var identityMock = new Mock<IIdentityInfo>();

        // Not a SuperAdmin
        identityMock.Setup(x => x.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        identityMock.Setup(x => x.GetIdentityId()).Returns(123);

        var requirements = new AccessRequirements();
        requirements.SetRequirement(DataRightsEnum.Read);

        // Setup protection for the entity
        var securedData = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var protectedMock = new Mock<IProtected<JudgedQueriesDummyEntity>>();
        protectedMock.Setup(x => x.IsMatch(typeof(JudgedQueriesDummyEntity))).Returns(true);
        protectedMock.Setup(x => x.Secured(123, DataRightsEnum.Read)).Returns(securedData);

        var protections = new List<IProtected> { protectedMock.Object };

        var judgedQueries = new JudgedQueries<DbContext>(
            dbContextMock.Object,
            identityMock.Object,
            requirements,
            protections);

        // Act
        var result = judgedQueries.Secure<JudgedQueriesDummyEntity>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(securedData);
        identityMock.Verify(x => x.HasRole(ApplicationRoleEnum.SuperAdmin), Times.Once);
        identityMock.Verify(x => x.GetIdentityId(), Times.Once);
        protectedMock.Verify(x => x.IsMatch(typeof(JudgedQueriesDummyEntity)), Times.Once);
        protectedMock.Verify(x => x.Secured(123, DataRightsEnum.Read), Times.Once);
    }

    [Fact]
    public void Secure_WhenEntityHasNoMatchingProtection_ShouldReturnUnfilteredDbSet()
    {
        // Arrange
        var data = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity(),
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var dbContextMock = CreateDbContextMock(data);
        var identityMock = new Mock<IIdentityInfo>();

        // Not a SuperAdmin
        identityMock.Setup(x => x.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        identityMock.Setup(x => x.GetIdentityId()).Returns(456);

        var requirements = new AccessRequirements();
        requirements.SetRequirement(DataRightsEnum.Read);

        // Setup protection that doesn't match our entity
        var protectedMock = new Mock<IProtected>();
        protectedMock.Setup(x => x.IsMatch(typeof(JudgedQueriesDummyEntity))).Returns(false);

        var protections = new List<IProtected> { protectedMock.Object };

        var judgedQueries = new JudgedQueries<DbContext>(
            dbContextMock.Object,
            identityMock.Object,
            requirements,
            protections);

        // Act
        var result = judgedQueries.Secure<JudgedQueriesDummyEntity>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(dbContextMock.Object.Set<JudgedQueriesDummyEntity>());
        identityMock.Verify(x => x.HasRole(ApplicationRoleEnum.SuperAdmin), Times.Once);
        protectedMock.Verify(x => x.IsMatch(typeof(JudgedQueriesDummyEntity)), Times.Once);
    }

    [Fact]
    public void Secure_WhenMultipleProtections_ShouldUseMatchingProtection()
    {
        // Arrange
        var data = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity(),
                new JudgedQueriesDummyEntity(),
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var dbContextMock = CreateDbContextMock(data);
        var identityMock = new Mock<IIdentityInfo>();

        // Not a SuperAdmin
        identityMock.Setup(x => x.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        identityMock.Setup(x => x.GetIdentityId()).Returns(789);

        var requirements = new AccessRequirements();
        requirements.SetRequirement(DataRightsEnum.ReadWrite);

        // Setup protection that doesn't match our entity
        var nonMatchingProtectedMock = new Mock<IProtected>();
        nonMatchingProtectedMock.Setup(x => x.IsMatch(typeof(JudgedQueriesDummyEntity))).Returns(false);

        // Setup protection that matches our entity
        var securedData = new List<JudgedQueriesDummyEntity>
            {
                new JudgedQueriesDummyEntity()
            }.AsQueryable();

        var matchingProtectedMock = new Mock<IProtected<JudgedQueriesDummyEntity>>();
        matchingProtectedMock.Setup(x => x.IsMatch(typeof(JudgedQueriesDummyEntity))).Returns(true);
        matchingProtectedMock.Setup(x => x.Secured(789, DataRightsEnum.ReadWrite)).Returns(securedData);

        var protections = new List<IProtected>
            {
                nonMatchingProtectedMock.Object,
                matchingProtectedMock.Object
            };

        var judgedQueries = new JudgedQueries<DbContext>(
            dbContextMock.Object,
            identityMock.Object,
            requirements,
            protections);

        // Act
        var result = judgedQueries.Secure<JudgedQueriesDummyEntity>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(securedData);
        identityMock.Verify(x => x.HasRole(ApplicationRoleEnum.SuperAdmin), Times.Once);
        identityMock.Verify(x => x.GetIdentityId(), Times.Once);
        nonMatchingProtectedMock.Verify(x => x.IsMatch(typeof(JudgedQueriesDummyEntity)), Times.Once);
        matchingProtectedMock.Verify(x => x.IsMatch(typeof(JudgedQueriesDummyEntity)), Times.Once);
        matchingProtectedMock.Verify(x => x.Secured(789, DataRightsEnum.ReadWrite), Times.Once);
    }
}
