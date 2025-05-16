using Catalog.Products.Constants;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.CreateProduct;

internal record CreateProductRequest(ProductDto Product);
internal record CreateProductResponse(Guid ProductId);

/// <summary>
/// Defines the API endpoint for creating a new product.
/// </summary>
internal class CreateProductEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost(ProductApiRoutes.CreateProduct, async (CreateProductRequest request, ISender sender) =>
        {
            var command = new CreateProductCommand(request.Product);

            var result = await sender.Send(command);

            var response = new CreateProductResponse(result.ProductId);

            return Results.Created($"{ProductApiRoutes.ProductsBase}/{response.ProductId}", response);
        });
    }
}
