using BuildingBlock.Application.Repositories;
using BuildingBlock.Application.Security.Contracts;
using BuildingBlock.Domain.Enums;
using BuildingBlock.Persistence.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Persistence.Repositories
{
    public class JudgedQueries<TCtx> : ISecureQuery where TCtx : DbContext
    {
        private readonly TCtx _context;
        private readonly IIdentityInfo _info;
        private readonly IEnumerable<IProtected> _protection;

        private readonly AccessRequirements _requirements;

        public JudgedQueries(TCtx context, IIdentityInfo info, AccessRequirements requirements, IEnumerable<IProtected> protection)
        {
            _context = context;
            _info = info;
            _protection = protection;

            _requirements = requirements;
        }

        public IQueryable<T> Secure<T>() where T : class
        {
            if (_info.HasRole(ApplicationRoleEnum.SuperAdmin))
                return _context.Set<T>();

            if (_protection.FirstOrDefault(x => x.IsMatch(typeof(T))) is IProtected<T> entityLock)
                return entityLock.Secured(_info.GetIdentityId(), _requirements.Requirement);

            return _context.Set<T>();
        }
    }
}
