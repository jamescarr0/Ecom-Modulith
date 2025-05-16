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
public class CreateProductEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost(ApiRoutes.ProductsBase, async (CreateProductRequest request, ISender sender) =>
        {
            // Map the Product Dto to a Create Product Command which can be consumed by the associated handler
            var command = new CreateProductCommand(request.Product);

            // Send Command via MediatR 
            var result = await sender.Send(command);

            // Map create product result to the create product response
            var response = new CreateProductResponse(result.ProductId);

            return Results.Created($"{ApiRoutes.ProductsBase}/{response.ProductId}", response);
        });
    }
}
