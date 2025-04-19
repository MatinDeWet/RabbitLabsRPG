using System.Security.Claims;
using BuildingBlock.Application.Security;
using Shouldly;

namespace BuildingBlock.Application.Tests.Tests.Security;

public class InfoSetterTests
{
    [Fact]
    public void SetUser_ShouldReplaceExistingClaims()
    {
        // Arrange
        var initialClaims = new List<Claim>
        {
            new (ClaimTypes.Name, "OldUser")
        };
        var newClaims = new List<Claim>
        {
            new (ClaimTypes.Name, "NewUser")
        };

        var infoSetter = new InfoSetter();
        infoSetter.AddRange(initialClaims);

        // Act
        infoSetter.SetUser(newClaims);

        // Assert
        infoSetter.ShouldContain(newClaims.First());
        infoSetter.ShouldNotContain(initialClaims.First());
    }

    [Fact]
    public void Clear_ShouldRemoveAllClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, "User")
        };

        var infoSetter = new InfoSetter();
        infoSetter.AddRange(claims);

        // Act
        infoSetter.Clear();

        // Assert
        infoSetter.ShouldBeEmpty();
    }

    [Fact]
    public void AddRange_ShouldAddMultipleClaims()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, "User1"),
            new (ClaimTypes.Role, "Admin")
        };

        var infoSetter = new InfoSetter();

        // Act
        infoSetter.AddRange(claims);

        // Assert
        infoSetter.ShouldContain(claims[0]);
        infoSetter.ShouldContain(claims[1]);
    }
}
