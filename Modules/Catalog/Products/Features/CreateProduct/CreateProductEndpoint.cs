using Catalog.Products.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Web;

namespace Catalog.Products.Features.CreateProduct;

internal record CreateProductRequest(ProductDto ProductDto);
internal record CreateProductResponse(Guid ProductId);


public class CreateProductEndpoint : IEndpointDefinition
{
    public void RegisterEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/products", () => Results.Ok("I AM WORKING!"));
    }
}
