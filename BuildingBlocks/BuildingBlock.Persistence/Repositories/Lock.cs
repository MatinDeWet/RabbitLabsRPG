using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories.Contracts;
using BuildingBlock.Persistence.Repositories.Enums;

namespace BuildingBlock.Persistence.Repositories;

public abstract class Lock<T> : IProtected<T> where T : class
{
    public abstract IQueryable<T> Secured(int identityId, DataRightsEnum requirement);

    public abstract Task<bool> HasAccess(T obj, int identityId, RepositoryOperationEnum operation, DataRightsEnum requirement, CancellationToken cancellationToken);

    public virtual bool IsMatch(Type t)
    {
        return typeof(T).IsAssignableFrom(t);
    }
}
