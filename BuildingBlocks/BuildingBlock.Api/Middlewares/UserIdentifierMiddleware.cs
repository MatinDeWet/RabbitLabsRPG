using System.Security.Claims;
using BuildingBlock.Application.Security.Contracts;
using Microsoft.AspNetCore.Http;

namespace BuildingBlock.Api.Middlewares;

public class UserIdentifierMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdentifierMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context, IInfoSetter infoSetter)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(infoSetter, nameof(infoSetter));

        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            IEnumerable<Claim> claims = context.User.Claims;

            infoSetter.SetUser(claims);
        }

        return _next(context);
    }
}
