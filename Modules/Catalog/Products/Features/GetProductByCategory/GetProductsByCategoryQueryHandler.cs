using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS.Query;

namespace Catalog.Products.Features.GetProductByCategory;

internal record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
internal record GetProductsByCategoryResult(IReadOnlyList<ProductDto> ProductsDtoList);

internal class GetProductsByCategoryQueryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var productsDto = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ProjectToType<ProductDto>()
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetProductsByCategoryResult(productsDto.AsReadOnly());
    }
}
