using BuildingBlock.Domain.Enums;

namespace BuildingBlock.Persistence.Repositories;

public class AccessRequirements
{
    /// <summary>
    /// Gets the current access requirement.
    /// </summary>
    /// <returns>The current group rights requirement.</returns>
    public DataRightsEnum Requirement { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the access requirement is set.
    /// </summary>
    public bool IsSet { get; private set; }

    public AccessRequirements()
    {
        Reset();
    }

    /// <summary>
    /// Resets the access requirement to the default value (Read) and marks it as not set.
    /// </summary>
    public void Reset()
    {
        SetRequirement(DataRightsEnum.Read);
        IsSet = false;
    }

    /// <summary>
    /// Sets the access requirement to the specified value.
    /// </summary>
    /// <param name="requirement">The required group rights for access.</param>
    /// <exception cref="InvalidOperationException">Thrown if the requirement is set to None.</exception>
    public void SetRequirement(DataRightsEnum requirement)
    {
        if (requirement == DataRightsEnum.None)
            throw new InvalidOperationException("Access Requirement 'None' is invalid");

        Requirement = requirement;
        IsSet = true;
    }
}
