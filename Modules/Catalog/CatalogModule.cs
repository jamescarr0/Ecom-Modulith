using System.Reflection;
using Catalog.Data;
using Catalog.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Seed;
using Shared.Interceptors;
using Shared.Web;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add application services to the container
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Add Data & Infrastructure services to the container
        services.AddScoped<IDbSeeder, CatalogDbSeeder>();
        services.AddScoped<ISaveChangesInterceptor, EntityAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDbContext>((sp, o) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            o.UseNpgsql(connectionString);
            o.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()); // Add the interceptors that were registered above as services
        });

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Use Data services
        app.UseMigration<CatalogDbContext>();

        // Register Endpoint Definitions
        app.UseEndpoints(endpoints =>
        {
            endpoints.RegisterEndpointDefinitions(typeof(CatalogModule).Assembly);
        });

        return app;
    }
}

