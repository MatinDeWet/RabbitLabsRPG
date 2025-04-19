using System.Security.Claims;

namespace BuildingBlock.Application.Security.Contracts;

/// <summary>
/// Represents a collection of claims that can be used to manage user identity information.
/// Extends the standard IList<Claim> interface with additional user identity management capabilities.
/// </summary>
public interface IInfoSetter : IList<Claim>
{
    /// <summary>
    /// Sets the current user's claims by replacing all existing claims with the provided collection.
    /// </summary>
    /// <param name="claims">The collection of claims to associate with the current user.</param>
    void SetUser(IEnumerable<Claim> claims);
}

