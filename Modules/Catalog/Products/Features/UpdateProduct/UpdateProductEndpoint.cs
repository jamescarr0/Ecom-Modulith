using Catalog.Products.Constants;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.UpdateProduct;

internal record UpdateProductRequest(ProductDto Product);
internal record UpdateProductResponse(bool IsSuccess);

/// <summary>
/// Defines the API endpoint for updating an existing product
/// </summary>
internal class UpdateProductEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPut(ProductApiRoutes.UpdateProduct, async (UpdateProductRequest request, ISender sender) =>
        {
            var command = new UpdateProductCommand(request.Product);
            
            var result = await sender.Send(command);
            
            var response = new UpdateProductResponse(result.IsSuccess);

            return Results.Ok(response);
        });
    }
}
