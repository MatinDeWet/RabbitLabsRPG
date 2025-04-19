using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories;
using Shouldly;

namespace BuildingBlock.Persistence.Tests.Tests.Repositories;

public class AccessRequirementsTests
{
    [Fact]
    public void Constructor_WhenCalled_ShouldResetToDefault()
    {
        // Arrange & Act
        var accessRequirements = new AccessRequirements();

        // Assert
        accessRequirements.Requirement.ShouldBe(DataRightsEnum.Read);
        accessRequirements.IsSet.ShouldBeFalse();
    }

    [Fact]
    public void SetRequirement_WhenCalled_WithValidValue_ShouldSetRequirementAndMarkIsSetTrue()
    {
        // Arrange
        var accessRequirements = new AccessRequirements();

        // Act
        accessRequirements.SetRequirement(DataRightsEnum.ReadWrite);

        // Assert
        accessRequirements.Requirement.ShouldBe(DataRightsEnum.ReadWrite);
        accessRequirements.IsSet.ShouldBeTrue();
    }

    [Fact]
    public void SetRequirement_WhenCalled_WithNone_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var accessRequirements = new AccessRequirements();

        // Act
        Action act = () => accessRequirements.SetRequirement(DataRightsEnum.None);

        // Assert
        act.ShouldThrow<InvalidOperationException>()
            .Message.ShouldBe("Access Requirement 'None' is invalid");
    }

    [Fact]
    public void Reset_WhenCalled_ShouldSetRequirementToReadAndMarkIsSetFalse()
    {
        // Arrange
        var accessRequirements = new AccessRequirements();
        accessRequirements.SetRequirement(DataRightsEnum.ReadWrite);
        accessRequirements.IsSet.ShouldBeTrue();

        // Act
        accessRequirements.Reset();

        // Assert
        accessRequirements.Requirement.ShouldBe(DataRightsEnum.Read);
        accessRequirements.IsSet.ShouldBeFalse();
    }
}
