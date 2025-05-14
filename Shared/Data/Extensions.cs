using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Data.Seed;

namespace Shared.Data;

public static class Extensions
{
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
        SeedDatabaseAsync(app.ApplicationServices).GetAwaiter().GetResult();
        return app;
    }

    private static async Task SeedDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (env.IsDevelopment())
        {
            var seeder = scope.ServiceProvider.GetServices<IDbSeeder>();

            foreach (var seed in seeder)
            {
                await seed.SeedAllAsync();
            }
        }

    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider services) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }
}
