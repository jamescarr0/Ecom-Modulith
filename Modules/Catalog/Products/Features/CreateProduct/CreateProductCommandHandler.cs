using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Command;

namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto ProductDto)
    : ICommand<CreateProductResult>;
internal record CreateProductResult(Guid ProductId);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductDto.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.ProductDto.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ProductDto.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ProductDto.ImageFile).NotEmpty().WithMessage("Image file is required");
        RuleFor(x => x.ProductDto.Price).GreaterThan(0).WithMessage("Price file is required");
    }
}

/// <summary>
/// Create Product Command Handler
/// - Creates the Product Entity and saves to the database
/// </summary>
internal class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Class}.{Method} called with {@Command}", nameof(CreateProductCommandHandler), nameof(Handle), command);

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