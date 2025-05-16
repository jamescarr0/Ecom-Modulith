using Catalog.Products.Constants;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.DeleteProduct;

internal record DeleteProductResponse(bool IsSuccess);

/// <summary>
/// Defines the API endpoint for deleting a product
/// The Product Guid is passed in the Url
/// </summary>
internal class DeleteProductEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapDelete(ProductApiRoutes.DeleteProduct, async (Guid ProductId, ISender sender) =>
        {
            var command = new DeleteProductCommand(ProductId);

            var result = await sender.Send(command);

            var response = new DeleteProductResponse(result.IsSuccess);

            return Results.Ok(response);
        });
    }
}
