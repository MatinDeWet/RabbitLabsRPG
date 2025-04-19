using BuildingBlock.Application.Behaviors;
using BuildingBlock.Application.Security;
using BuildingBlock.Application.Security.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.Application;

public static class BuildingBlockApplicationDI
{
    public static IServiceCollection AddDataIdentity(this IServiceCollection services)
    {
        services.AddScoped<IIdentityInfo, IdentityInfo>();
        services.AddScoped<IInfoSetter, InfoSetter>();

        return services;
    }

    public static IServiceCollection AddMediatRBehavior(this IServiceCollection services, Type pointerType)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(pointerType.Assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return services;
    }
}
