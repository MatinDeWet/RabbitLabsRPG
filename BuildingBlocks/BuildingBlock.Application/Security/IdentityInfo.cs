using System.Security.Claims;
using BuildingBlock.Application.Security.Contracts;
using BuildingBlock.Domain.Enums;

namespace BuildingBlock.Application.Security;

public class IdentityInfo(IInfoSetter infoSetter) : IIdentityInfo
{
    public int GetIdentityId()
    {
        string uid = GetValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(uid, out int result))
            return 0;

        return result;
    }

    public bool HasRole(ApplicationRoleEnum role)
    {
        var roles = infoSetter
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();

        ApplicationRoleEnum combinedRoles = ApplicationRoleEnum.None;

        foreach (string roleString in roles)
            if (Enum.TryParse(roleString, true, out ApplicationRoleEnum parsedRole))
                combinedRoles |= parsedRole;

        return combinedRoles.HasFlag(role);
    }

    public string GetValue(string name)
    {
        Claim? claim = infoSetter.FirstOrDefault(x => x.Type == name);
        return claim == null ? null! : claim.Value;
    }

    public bool HasValue(string name)
    {
        return infoSetter.Any(x => x.Type == name);
    }
}
