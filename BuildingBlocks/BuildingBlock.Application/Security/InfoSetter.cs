using System.Security.Claims;
using BuildingBlock.Application.Security.Contracts;

namespace BuildingBlock.Application.Security;

public class InfoSetter : List<Claim>, IInfoSetter
{
    public void SetUser(IEnumerable<Claim> claims)
    {
        Clear();
        AddRange(claims);
    }

    public virtual new void Clear()
    {
        base.Clear();
    }

    public virtual new void AddRange(IEnumerable<Claim> claims)
    {
        base.AddRange(claims);
    }
}
