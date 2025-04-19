using BuildingBlock.Application.Repositories;
using BuildingBlock.Application.Security.Contracts;
using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories.Contracts;
using BuildingBlock.Persistence.Repositories.Enums;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Persistence.Repositories;

public class JudgedCommands<TCtx> : ISecureCommand where TCtx : DbContext
{
    private readonly TCtx _context;
    private readonly IIdentityInfo _info;
    private readonly IEnumerable<IProtected> _protection;

    private readonly AccessRequirements _requirements;

    public JudgedCommands(TCtx context, IIdentityInfo info, AccessRequirements requirements, IEnumerable<IProtected> protection)
    {
        _context = context;
        _info = info;
        _protection = protection;

        _requirements = requirements;
    }

    public virtual async Task InsertAsync<T>(T obj, CancellationToken cancellationToken) where T : class
    {
        if (!_requirements.IsSet)
            _requirements.SetRequirement(DataRightsEnum.ReadWrite);

        bool hasAccess = await HasAccess(obj, RepositoryOperationEnum.Insert, cancellationToken);

        if (!hasAccess)
            throw new UnauthorizedAccessException();

        _context.Add(obj);
    }

    public async Task InsertAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class
    {
        await InsertAsync(obj, cancellationToken);

        if (persistImmediately)
            await SaveAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync<T>(T obj, CancellationToken cancellationToken) where T : class
    {
        if (!_requirements.IsSet)
            _requirements.SetRequirement(DataRightsEnum.ReadWrite);

        bool hasAccess = await HasAccess(obj, RepositoryOperationEnum.Update, cancellationToken);

        if (!hasAccess)
            throw new UnauthorizedAccessException();

        _context.Update(obj);
    }

    public async Task UpdateAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class
    {
        await UpdateAsync(obj, cancellationToken);

        if (persistImmediately)
            await SaveAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync<T>(T obj, CancellationToken cancellationToken) where T : class
    {
        if (!_requirements.IsSet)
            _requirements.SetRequirement(DataRightsEnum.ReadWrite);

        bool hasAccess = await HasAccess(obj, RepositoryOperationEnum.Delete, cancellationToken);

        if (!hasAccess)
            throw new UnauthorizedAccessException();

        _context.Remove(obj);
    }

    public async Task DeleteAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class
    {
        await DeleteAsync(obj, cancellationToken);

        if (persistImmediately)
            await SaveAsync(cancellationToken);
    }

    public virtual async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if the current user has access to perform the specified operation on the entity.
    /// If the user has the SuperAdmin role, the access check is bypassed and returns true.
    /// </summary>
    /// <typeparam name="T">The type of the entity to check access for.</typeparam>
    /// <param name="obj">The entity to check access for.</param>
    /// <param name="operation">The type of repository operation being performed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if access is granted; otherwise, false.</returns>
    private async Task<bool> HasAccess<T>(T obj, RepositoryOperationEnum operation, CancellationToken cancellationToken) where T : class
    {
        bool result = true;

        if (_info.HasRole(ApplicationRoleEnum.SuperAdmin))
        {
            _requirements.Reset();

            return result;
        }

        if (_protection.FirstOrDefault(x => x.IsMatch(typeof(T))) is IProtected<T> entityLock)
        {
            result = await entityLock.HasAccess(obj, _info.GetIdentityId(), operation, _requirements.Requirement, cancellationToken);
        }

        // Reset back to Reading
        _requirements.Reset();

        return result;
    }
}
