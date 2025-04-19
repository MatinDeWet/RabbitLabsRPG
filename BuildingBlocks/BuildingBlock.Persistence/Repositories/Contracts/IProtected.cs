using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories.Enums;

namespace BuildingBlock.Persistence.Repositories.Contracts;

public interface IProtected
{
    /// <summary>
    /// Determines if the specified type matches the entity to be protected.
    /// </summary>
    /// <param name="t">The type to check against the protected entity.</param>
    /// <returns>True if the type matches; otherwise, false.</returns>
    bool IsMatch(Type t);
}

public interface IProtected<T> : IProtected where T : class
{
    /// <summary>
    /// Applies security measures and returns a queryable collection of the protected entity.
    /// </summary>
    /// <param name="identityId">The identity ID of the user requesting access.</param>
    /// <param name="requirement">The required group rights for access.</param>
    /// <returns>A queryable collection of the protected entity.</returns>
    IQueryable<T> Secured(int identityId, DataRightsEnum requirement); // for Read

    /// <summary>
    /// Verifies access to the specified entity based on the provided requirements and operation.
    /// </summary>
    /// <param name="obj">The entity to verify access for.</param>
    /// <param name="identityId">The identity ID of the user requesting access.</param>
    /// <param name="operation">The type of repository operation being performed.</param>
    /// <param name="requirement">The required group rights for access.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if access is granted; otherwise, false.</returns>
    Task<bool> HasAccess(T obj, int identityId, RepositoryOperationEnum operation, DataRightsEnum requirement, CancellationToken cancellationToken); // for Write
}
