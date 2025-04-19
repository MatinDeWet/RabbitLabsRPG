using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using Serilog;

namespace BuildingBlock.Api;

public static class BuildingBlockApiDI
{
    public static IServiceCollection PrepareFastEndpoints(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        string? signingKey = configuration.GetValue<string>("ScalarExplorer:Title");

        services.AddFastEndpoints()
            .AddAuthenticationJwtBearer(s => s.SigningKey = signingKey)
            .AddAuthorization()
            .SwaggerDocument(o =>
            {
                o.ShortSchemaNames = true;
            });

        return services;
    }

    public static IApplicationBuilder UseFastEndpoints(this IApplicationBuilder app)
    {
        app.UseFastEndpoints();
        app.UseSwaggerGen();

        return app;
    }

    public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder builder)
    {
        builder.UseAuthentication()
            .UseAuthorization();

        return builder;
    }

    public static IEndpointRouteBuilder UseScalarApiDocumentation(this IEndpointRouteBuilder builder, bool isLocked, IConfiguration configuration)
    {
        if (!isLocked)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            List<ScalarServer>? servers = configuration.GetSection("ApiSpecification:Servers").Get<List<ScalarServer>>();
            string? title = configuration.GetValue<string>("ScalarExplorer:Title");


            builder.MapScalarApiReference(options =>
            {
                options.WithTitle(title!)
                .WithTheme(ScalarTheme.Moon)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                .WithDarkMode(true)
                .WithPreferredScheme("Bearer")
                .WithHttpBearerAuthentication(bearer =>
                {
                    bearer.Token = "your-bearer-token";
                });

                options.Servers = servers;
            });
        }

        return builder;
    }

    public static IHostBuilder AddSerilog(this IHostBuilder webBuilder)
    {
        webBuilder.UseSerilog((ctx, lc) =>
        {
            lc.ReadFrom.Configuration(ctx.Configuration);
        });

        return webBuilder;
    }

    public static IApplicationBuilder UseSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }

    public static IApplicationBuilder AddCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseDefaultExceptionHandler();

        return app;
    }
}
