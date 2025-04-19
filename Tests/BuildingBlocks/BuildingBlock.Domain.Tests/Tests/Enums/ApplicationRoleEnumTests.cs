using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BuildingBlock.Domain.Enums;
using Shouldly;

namespace BuildingBlock.Domain.Tests.Tests.Enums;

public class ApplicationRoleEnumTests
{
    [Fact]
    public void ApplicationRoleEnum_Values_ShouldHaveCorrectIntegerValues()
    {
        // Assert
        ((int)ApplicationRoleEnum.None).ShouldBe(0);
        ((int)ApplicationRoleEnum.Admin).ShouldBe(1);
        ((int)ApplicationRoleEnum.SuperAdmin).ShouldBe(3); // 1 | 2 = 3
    }

    [Fact]
    public void ApplicationRoleEnum_DisplayAttributes_ShouldHaveCorrectNames()
    {
        // Arrange & Act
        string noneName = GetDisplayName(ApplicationRoleEnum.None);
        string adminName = GetDisplayName(ApplicationRoleEnum.Admin);
        string superAdminName = GetDisplayName(ApplicationRoleEnum.SuperAdmin);

        // Assert
        noneName.ShouldBe("None");
        adminName.ShouldBe("Admin");
        superAdminName.ShouldBe("Super Admin");
    }

    [Fact]
    public void SuperAdmin_ShouldIncludeAdminRole()
    {
        // Arrange
        ApplicationRoleEnum superAdmin = ApplicationRoleEnum.SuperAdmin;

        // Act & Assert
        superAdmin.HasFlag(ApplicationRoleEnum.Admin).ShouldBeTrue();
    }

    [Fact]
    public void Admin_ShouldNotIncludeSuperAdminRole()
    {
        // Arrange
        ApplicationRoleEnum admin = ApplicationRoleEnum.Admin;

        // Act & Assert
        admin.HasFlag(ApplicationRoleEnum.SuperAdmin).ShouldBeFalse();
    }

    [Fact]
    public void None_ShouldNotIncludeAnyOtherRole()
    {
        // Arrange
        ApplicationRoleEnum none = ApplicationRoleEnum.None;

        // Act & Assert
        none.HasFlag(ApplicationRoleEnum.Admin).ShouldBeFalse();
        none.HasFlag(ApplicationRoleEnum.SuperAdmin).ShouldBeFalse();
    }

    [Fact]
    public void BitwiseOR_ShouldCombineRoles()
    {
        // Arrange
        ApplicationRoleEnum combined = ApplicationRoleEnum.Admin | ApplicationRoleEnum.None;

        // Act & Assert
        combined.ShouldBe(ApplicationRoleEnum.Admin);
        combined.HasFlag(ApplicationRoleEnum.Admin).ShouldBeTrue();
    }

    [Fact]
    public void BitwiseAND_ShouldExtractCommonRoles()
    {
        // Arrange & Act
        ApplicationRoleEnum common = ApplicationRoleEnum.SuperAdmin & ApplicationRoleEnum.Admin;

        // Assert
        common.ShouldBe(ApplicationRoleEnum.Admin);
    }

    [Fact]
    public void AllDefinedValues_ShouldBeSupported()
    {
        // This test ensures all enum values are considered in our tests
        ApplicationRoleEnum[] allDefinedValues = Enum.GetValues<ApplicationRoleEnum>();

        // Assert we're testing all values
        allDefinedValues.Length.ShouldBe(3); // None, Admin, SuperAdmin
        allDefinedValues.ShouldContain(ApplicationRoleEnum.None);
        allDefinedValues.ShouldContain(ApplicationRoleEnum.Admin);
        allDefinedValues.ShouldContain(ApplicationRoleEnum.SuperAdmin);
    }

    // Helper method to get the display name from Display attribute
    private static string GetDisplayName(ApplicationRoleEnum value)
    {
        // Get the enum field
        FieldInfo? field = typeof(ApplicationRoleEnum).GetField(value.ToString());

        if (field == null)
            return value.ToString();

        // Get the Display attribute
        DisplayAttribute? attribute = field.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? value.ToString();
    }
}
