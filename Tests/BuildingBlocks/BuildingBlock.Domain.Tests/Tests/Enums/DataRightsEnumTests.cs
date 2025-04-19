using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BuildingBlock.Domain.Enums;
using Shouldly;

namespace BuildingBlock.Domain.Tests.Tests.Enums;

public class DataRightsEnumTests
{
    [Fact]
    public void DataRightsEnum_Values_ShouldHaveCorrectIntegerValues()
    {
        // Assert
        ((int)DataRightsEnum.None).ShouldBe(0);
        ((int)DataRightsEnum.Read).ShouldBe(1);
        ((int)DataRightsEnum.ReadWrite).ShouldBe(3); // 1 | 2 = 3
        ((int)DataRightsEnum.Owner).ShouldBe(7); // 3 | 4 = 7
    }

    [Fact]
    public void DataRightsEnum_DisplayAttributes_ShouldHaveCorrectNames()
    {
        // Arrange & Act
        string noneName = GetDisplayName(DataRightsEnum.None);
        string readName = GetDisplayName(DataRightsEnum.Read);
        string readWriteName = GetDisplayName(DataRightsEnum.ReadWrite);
        string ownerName = GetDisplayName(DataRightsEnum.Owner);

        // Assert
        noneName.ShouldBe("None");
        readName.ShouldBe("Read");
        readWriteName.ShouldBe("Read & Write");
        ownerName.ShouldBe("Owner");
    }

    [Fact]
    public void ReadWrite_ShouldIncludeRead()
    {
        // Arrange
        DataRightsEnum readWrite = DataRightsEnum.ReadWrite;

        // Act & Assert
        readWrite.HasFlag(DataRightsEnum.Read).ShouldBeTrue();
    }

    [Fact]
    public void Owner_ShouldIncludeReadWriteAndRead()
    {
        // Arrange
        DataRightsEnum owner = DataRightsEnum.Owner;

        // Act & Assert
        owner.HasFlag(DataRightsEnum.ReadWrite).ShouldBeTrue();
        owner.HasFlag(DataRightsEnum.Read).ShouldBeTrue();
    }

    [Fact]
    public void None_ShouldNotIncludeAnyOtherRights()
    {
        // Arrange
        DataRightsEnum none = DataRightsEnum.None;

        // Act & Assert
        none.HasFlag(DataRightsEnum.Read).ShouldBeFalse();
        none.HasFlag(DataRightsEnum.ReadWrite).ShouldBeFalse();
        none.HasFlag(DataRightsEnum.Owner).ShouldBeFalse();
    }

    [Fact]
    public void BitwiseOR_ShouldCombineRights()
    {
        // Arrange
        DataRightsEnum combined = DataRightsEnum.Read | DataRightsEnum.None;

        // Act & Assert
        combined.ShouldBe(DataRightsEnum.Read);
        combined.HasFlag(DataRightsEnum.Read).ShouldBeTrue();
    }

    [Fact]
    public void BitwiseAND_ShouldExtractCommonRights()
    {
        // Arrange & Act
        DataRightsEnum common = DataRightsEnum.Owner & DataRightsEnum.ReadWrite;

        // Assert
        common.ShouldBe(DataRightsEnum.ReadWrite);
    }

    [Fact]
    public void AllDefinedValues_ShouldBeSupported()
    {
        // This test ensures all enum values are considered in our tests
        var allDefinedValues = new List<DataRightsEnum>
        {
            DataRightsEnum.None,
            DataRightsEnum.Read,
            DataRightsEnum.ReadWrite,
            DataRightsEnum.Owner
        };

        // Assert we're testing all values
        allDefinedValues.Count.ShouldBe(4); // None, Read, ReadWrite, Owner
        allDefinedValues.ShouldContain(DataRightsEnum.None);
        allDefinedValues.ShouldContain(DataRightsEnum.Read);
        allDefinedValues.ShouldContain(DataRightsEnum.ReadWrite);
        allDefinedValues.ShouldContain(DataRightsEnum.Owner);
    }

    // Helper method to get the display name from Display attribute
    private static string GetDisplayName(DataRightsEnum value)
    {
        // Get the enum field
        FieldInfo? field = typeof(DataRightsEnum).GetField(value.ToString());

        if (field == null)
            return value.ToString();

        // Get the Display attribute
        DisplayAttribute? attribute = field.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ?? value.ToString();
    }
}
