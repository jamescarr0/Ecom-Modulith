using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Exceptions;
using Catalog.Products.Models;
using FluentValidation;
using Shared.CQRS.Command;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;
internal record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("Image file is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price file is required");
    }
}

internal class UpdateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.Product.Id], cancellationToken: cancellationToken)
            ?? throw new ProductNotFoundException(command.Product.Id);
        
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
