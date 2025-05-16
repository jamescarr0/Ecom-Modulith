using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Shared.CQRS.Command;

namespace Catalog.Products.Features.UpdateProduct;

internal record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;
internal record UpdateProductResult(bool IsSuccess);

internal class UpdateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.Product.Id], cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found with Id: {command.Product.Id}");
        }

        UpdateProduct(product, command.Product);

        dbContext.Products.Update(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private void UpdateProduct(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price);
    }
}
