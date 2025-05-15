using MediatR;

namespace Catalog.Products.Features.CreateProduct;

internal record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : IRequest<CreateProductResult>;

internal record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create the product.
        throw new NotImplementedException();
    }
}