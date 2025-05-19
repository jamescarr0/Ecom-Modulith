using Basket;
using Catalog;
using Ordering;
using Shared.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP Pipeline
app.UseRouting();

app.UseExceptionHandler(o => { });

app.UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.Run();
