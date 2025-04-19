using BuildingBlock.Application.Repositories;
using BuildingBlock.Persistence.Repositories;
using BuildingBlock.Persistence.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.Persistence;

public static class BuildingBlockPersistenceDI
{
    public static IServiceCollection AddDataSecurity(this IServiceCollection services)
    {
        services.AddScoped<AccessRequirements>();

        return services;
    }

    public static IServiceCollection AddRepos(this IServiceCollection services, Type assemblyPointer)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(ISecureQuery), typeof(ISecureCommand)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddRepoLocks(this IServiceCollection services, Type assemblyPointer)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(IProtected)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
