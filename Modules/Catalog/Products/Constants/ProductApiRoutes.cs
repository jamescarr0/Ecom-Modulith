﻿namespace Catalog.Products.Constants;

internal class ProductApiRoutes
{
    public const string ProductsBase = "/products";

    public const string GetProducts = $"{ProductsBase}";
    public const string GetProductById = $"{ProductsBase}/id/{{ProductId}}";
    public const string GetProductByCategory = $"{ProductsBase}/category/{{ProductCategory}}";
    public const string CreateProduct = $"{ProductsBase}/create";
    public const string UpdateProduct = $"{ProductsBase}/update";
    public const string DeleteProduct = $"{ProductsBase}/delete/{{ProductId}}";
}
