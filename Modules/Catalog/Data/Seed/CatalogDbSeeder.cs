using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Data.Seed;

namespace Catalog.Data.Seed;

public class CatalogDbSeeder(CatalogDbContext dbContext) : IDbSeeder
{
    public async Task SeedAllAsync()
    {
        await dbContext.Database.EnsureCreatedAsync();

        if(!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(InitialSeedData.Products);
            await dbContext.SaveChangesAsync();
        }
    }
}
