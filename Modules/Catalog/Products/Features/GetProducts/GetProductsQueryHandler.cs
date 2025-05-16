using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts;

internal record GetProductsQuery() : IQuery<GetProductsResult>;
internal record GetProductsResult(IReadOnlyList<ProductDto> ProductsDtoList);

internal class GetProductsQueryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productsDtoList = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ProjectToType<ProductDto>()
            .ToListAsync(cancellationToken);

        return new GetProductsResult(productsDtoList.AsReadOnly());
    }
}
