using Catalog.Products.Constants;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.GetProducts;

internal record GetProductsResponse(IReadOnlyList<ProductDto> Products);

internal class GetProductsEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet(ProductApiRoutes.GetProducts, async (ISender sender) =>
        {
            var query = new GetProductsQuery();

            var result = await sender.Send(query);

            var response = new GetProductsResponse(result.ProductsDtoList);

            return Results.Ok(response);
        });
    }
}
