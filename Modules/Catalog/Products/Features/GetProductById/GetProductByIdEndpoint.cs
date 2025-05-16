using Catalog.Products.Constants;
using Catalog.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.GetProductById;

internal record GetProductByIdResponse(ProductDto Product);

internal class GetProductByIdEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet(ProductApiRoutes.GetProductById, async (Guid ProductId, ISender sender) =>
        {
            var query = new GetProductByIdQuery(ProductId);

            var result = await sender.Send(query);

            var response = new GetProductByIdResponse(result.ProductDto);

            return Results.Ok(response);
        });
    }
}
