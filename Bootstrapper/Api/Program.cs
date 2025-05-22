using Basket;
using Catalog;
using Ordering;
using Serilog;
using Shared.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP Pipeline
app.UseRouting();
app.UseExceptionHandler(o => { });

app.UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.Run();
