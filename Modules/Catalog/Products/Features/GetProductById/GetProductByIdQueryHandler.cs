using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS.Query;

namespace Catalog.Products.Features.GetProductById;

internal record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
internal record GetProductByIdResult(ProductDto ProductDto);

internal class GetProductByIdQueryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var productDto = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == query.ProductId)
            .ProjectToType<ProductDto>()
            .SingleOrDefaultAsync(cancellationToken);

        if(productDto is null)
        {
            throw new Exception($"Product not found with Id: {query.ProductId}");
        }

        return new GetProductByIdResult(productDto);
    }
}
