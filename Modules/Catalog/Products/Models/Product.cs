using Catalog.Products.Events;
using Shared.DDD;

namespace Catalog.Products.Models;
public class Product : Aggregate<Guid>
{
    public string Name { get; private set; }
    public List<string> Category { get; private set; } = new();
    public string Description { get; private set; }
    public string ImageFile { get; private set; }
    public decimal Price { get; private set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="Product"/> class while enforcing business validation rules.
    /// </summary>
    /// <param name="id">Unique identifier for the product.</param>
    /// <param name="name">Product name.</param>
    /// <param name="category">List of categories the product belongs to.</param>
    /// <param name="description">Detailed description of the product.</param>
    /// <param name="imageFile">Path to the product image file.</param>
    /// <param name="price">Product price (must be positive).</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="price"/> is zero or negative.</exception>
    public Product(Guid id, string name, List<string> category, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Id = id;
        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;

        AddDomainEvent(new ProductCreatedEvent(this));
    }

    /// <summary>
    /// Updates the current <see cref="Product"/> instance with new values, enforcing business validation rules.
    /// </summary>
    /// <param name="name">Updated product name (required).</param>
    /// <param name="category">Updated list of categories the product belongs to.</param>
    /// <param name="description">Updated detailed description of the product.</param>
    /// <param name="imageFile">Updated path to the product image file.</param>
    /// <param name="price">Updated product price (must be positive).</param>
    /// <exception cref="ArgumentException"> Thrown if <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown if <paramref name="price"/> is zero or negative.</exception>
    public void Update(string name, List<string> category, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;

        if (Price != price)
        {
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }

}

