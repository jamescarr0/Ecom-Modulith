namespace Catalog.Products.Dtos;

internal record ProductDto(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
    );
