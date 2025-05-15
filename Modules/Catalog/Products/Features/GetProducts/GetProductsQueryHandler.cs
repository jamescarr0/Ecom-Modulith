using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts;

internal record GetProductsQuery() : IQuery<GetProductsResult>;
internal record GetProductsResult(IEnumerable<ProductDto> ProductsDto);

internal class GetProductsQueryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var productsDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(productsDtos);
    }
}
