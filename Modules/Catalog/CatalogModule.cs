using Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container

        // Add Data & Infrastructure services
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CatalogDbContext>(o => o.UseNpgsql(connectionString));

        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        // Use Data services
        app.UseMigration<CatalogDbContext>();

        return app;
    }
}

