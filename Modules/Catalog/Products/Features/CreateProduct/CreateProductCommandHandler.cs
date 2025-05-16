using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct;

internal record CreateProductCommand(ProductDto ProductDto)
    : ICommand<CreateProductResult>;

internal record CreateProductResult(Guid ProductId);

/// <summary>
/// Create Product Command Handler
/// - Creates the Product Entity and saves to the database
/// </summary>
internal class CreateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.ProductDto);

        dbContext.Products.Add(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto productDto)
    {
        return new Product(
            Guid.NewGuid(),
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);
    }
}