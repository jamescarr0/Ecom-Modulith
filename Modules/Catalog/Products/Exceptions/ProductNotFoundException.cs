using Shared.Exceptions;

namespace Catalog.Products.Exceptions;

internal class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id) : base("Product", id)
    {
    }
}
