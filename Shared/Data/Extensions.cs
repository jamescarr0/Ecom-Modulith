using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Data;

public static class Extensions
{
    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        MigrateDatabase<TContext>(app.ApplicationServices);
        return app;
    }

    private static void MigrateDatabase<TContext>(IServiceProvider serviceProvider) where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        context.Database.Migrate();
    }
}
