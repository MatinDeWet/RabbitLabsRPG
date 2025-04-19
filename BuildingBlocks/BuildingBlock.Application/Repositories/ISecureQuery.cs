namespace BuildingBlock.Application.Repositories;

public interface ISecureQuery
{
    /// <summary>
    /// Returns a queryable collection of the entity with access predicates applied.
    /// If the entity is protected, security measures are applied based on the user's identity and requirements.
    /// If the user has the SuperAdmin role, the security measures is bypassed.
    /// </summary>
    /// <typeparam name="T">The type of the entity to secure.</typeparam>
    /// <returns>A queryable collection of the secured entity.</returns>
    IQueryable<T> Secure<T>() where T : class;
}
