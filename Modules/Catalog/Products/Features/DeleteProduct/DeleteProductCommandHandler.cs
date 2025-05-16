using Catalog.Data;
using Shared.CQRS.Command;

namespace Catalog.Products.Features.DeleteProduct;

internal record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;
internal record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found with Id: {command.ProductId}");
        }

        dbContext.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);

    }
}
