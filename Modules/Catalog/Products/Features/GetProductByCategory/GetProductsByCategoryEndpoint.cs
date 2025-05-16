using Catalog.Products.Constants;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.GetProductByCategory;

internal record GetProductsByCategoryResponse(IReadOnlyList<ProductDto> Products);

/// <summary>
/// Defines the Get Products By Category API endpoint route.
/// - Category string is passed in the url
/// </summary>
internal class GetProductsByCategoryEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet(ProductApiRoutes.GetProductByCategory, async (string ProductCategory, ISender sender) =>
        {
            var query = new GetProductsByCategoryQuery(ProductCategory);

            var result = await sender.Send(query);

            var response = new GetProductsByCategoryResponse(result.ProductsDtoList);

            return Results.Ok(response);
        });
    }
}
