using System.Security.Claims;
using BuildingBlock.Application.Security;
using BuildingBlock.Application.Security.Contracts;
using BuildingBlock.Domain.Enums;
using Moq;
using Shouldly;

namespace BuildingBlock.Application.Tests.Tests.Security;

public class IdentityInfoTests
{
    [Fact]
    public void GetIdentityId_ShouldReturnCorrectId_WhenValidClaimExists()
    {
        // Arrange
        var nameIdClaim = new Claim(ClaimTypes.NameIdentifier, "123");
        var mockInfoSetter = new Mock<IInfoSetter>();

        // Setup the mock to return our test claim when indexed
        mockInfoSetter.Setup(x => x.GetEnumerator())
                      .Returns(new List<Claim> { nameIdClaim }.GetEnumerator());

        var identityInfo = new IdentityInfo(mockInfoSetter.Object);

        // Act
        int result = identityInfo.GetIdentityId();

        // Assert
        result.ShouldBe(123);
    }

    [Fact]
    public void GetIdentityId_ShouldReturnZero_WhenClaimIsInvalid()
    {
        // Arrange
        var nameIdClaim = new Claim(ClaimTypes.NameIdentifier, "invalid");
        var mockInfoSetter = new Mock<IInfoSetter>();

        mockInfoSetter.Setup(x => x.GetEnumerator())
                      .Returns(new List<Claim> { nameIdClaim }.GetEnumerator());

        var identityInfo = new IdentityInfo(mockInfoSetter.Object);

        // Act
        int result = identityInfo.GetIdentityId();

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public void HasRole_ShouldReturnTrue_WhenRoleExists()
    {
        // Arrange
        var roleClaim = new Claim(ClaimTypes.Role, "Admin");
        var mockInfoSetter = new Mock<IInfoSetter>();

        mockInfoSetter.Setup(x => x.GetEnumerator())
                      .Returns(new List<Claim> { roleClaim }.GetEnumerator());

        var identityInfo = new IdentityInfo(mockInfoSetter.Object);

        // Act
        bool result = identityInfo.HasRole(ApplicationRoleEnum.Admin);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void GetValue_ShouldReturnCorrectValue_WhenClaimExists()
    {
        // Arrange
        var customClaim = new Claim("CustomClaim", "CustomValue");
        var mockInfoSetter = new Mock<IInfoSetter>();

        mockInfoSetter.Setup(x => x.GetEnumerator())
                      .Returns(new List<Claim> { customClaim }.GetEnumerator());

        var identityInfo = new IdentityInfo(mockInfoSetter.Object);

        // Act
        string result = identityInfo.GetValue("CustomClaim");

        // Assert
        result.ShouldBe("CustomValue");
    }

    [Fact]
    public void HasValue_ShouldReturnTrue_WhenClaimExists()
    {
        // Arrange
        var customClaim = new Claim("CustomClaim", "CustomValue");
        var mockInfoSetter = new Mock<IInfoSetter>();

        mockInfoSetter.Setup(x => x.GetEnumerator())
                      .Returns(new List<Claim> { customClaim }.GetEnumerator());

        var identityInfo = new IdentityInfo(mockInfoSetter.Object);

        // Act
        bool result = identityInfo.HasValue("CustomClaim");

        // Assert
        result.ShouldBeTrue();
    }
}
