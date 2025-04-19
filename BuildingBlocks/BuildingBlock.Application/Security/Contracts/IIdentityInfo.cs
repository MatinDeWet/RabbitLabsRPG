using BuildingBlock.Domain.Enums;

namespace BuildingBlock.Application.Security.Contracts;

public interface IIdentityInfo
{
    /// <summary>
    /// Retrieves the current user's identity ID as an integer.
    /// </summary>
    /// <returns>The user's ID as an integer, or 0 if the ID is not present or cannot be parsed.</returns>
    int GetIdentityId();

    /// <summary>
    /// Checks if the current user has the specified application role.
    /// Uses bitwise flags to support composite role verification.
    /// </summary>
    /// <param name="role">The role to check against the user's assigned roles.</param>
    /// <returns>True if the user has the specified role; otherwise, false.</returns>
    bool HasRole(ApplicationRoleEnum role);

    /// <summary>
    /// Determines whether a claim with the specified type exists.
    /// </summary>
    /// <param name="name">The claim type to check for.</param>
    /// <returns>True if a claim with the specified type exists; otherwise, false.</returns>
    bool HasValue(string name);

    /// <summary>
    /// Retrieves the value of a claim with the specified type.
    /// </summary>
    /// <param name="name">The claim type to retrieve the value for.</param>
    /// <returns>The value of the claim if it exists; otherwise, null.</returns>
    string GetValue(string name);

}
