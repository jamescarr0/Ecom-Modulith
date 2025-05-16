using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Features.CreateProduct;
using Catalog.Products.Models;
using FluentValidation;
using Shared.CQRS.Command;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto ProductDto) : ICommand<UpdateProductResult>;
internal record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductDto.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.ProductDto.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ProductDto.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ProductDto.ImageFile).NotEmpty().WithMessage("Image file is required");
        RuleFor(x => x.ProductDto.Price).GreaterThan(0).WithMessage("Price file is required");
    }
}

internal class UpdateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductDto.Id], cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found with Id: {command.ProductDto.Id}");
        }

        UpdateProduct(product, command.ProductDto);

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
