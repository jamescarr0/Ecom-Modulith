using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductByCategory;

internal record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
internal record GetProductsByCategoryResult(IEnumerable<ProductDto> ProductDto);

internal class GetProductsByCategoryQueryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var productsDto = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .Select(p => p.Adapt<ProductDto>())
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetProductsByCategoryResult(productsDto);
    }
}
