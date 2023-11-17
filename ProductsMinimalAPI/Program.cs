using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsMinimalAPI.Context;
using ProductsMinimalAPI.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// DbContext configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Categories endpoint
// CREATE category
app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext db) =>
{
    await db.Categories.AddAsync(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.CategoryId}", category);
}).Accepts<Category>("application/json")
    .Produces<Category>(StatusCodes.Status201Created)
    .WithName("CreateNewCategory")
    .WithTags("Categories");

// READ all categories
app.MapGet("/categories", async (AppDbContext db) =>
{
    return await db.Categories.ToListAsync();
}).WithTags("Categories"); ;

// READ category by ID
app.MapGet("/categories/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    return await db.Categories.FindAsync(id)
        is Category category ? Results.Ok(category) : Results.NotFound("Category not found.");
}).WithTags("Categories"); ;

// UPDATE category
app.MapPut("/categories/{id:int}", async ([FromRoute] int id,
                                          [FromBody] Category category,
                                          [FromServices] AppDbContext db) =>
{
    if (category.CategoryId != id) return Results.BadRequest();

    var categoryDB = await db.Categories.FindAsync(id);

    if (categoryDB is null) return Results.NotFound("Category not found");

    categoryDB.Name = category.Name;
    categoryDB.Description = category.Description;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Categories"); ;

// REMOVE category
app.MapDelete("categories/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);

    if (category is null) return Results.NotFound("Category not found.");

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithTags("Categories"); ;

// Products endpoint
// CREATE product
app.MapPost("/products", async ([FromBody] Product product, [FromServices] AppDbContext db) =>
{
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();

    return Results.Created($"/products/{product.ProductId}", product);
}).WithTags("Products");

// READ all products
app.MapGet("/products", async ([FromServices] AppDbContext db) =>
{
    return await db.Products.ToListAsync();
}).WithTags("Products");

// READ product by ID
app.MapGet("/products/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    return await db.Products.FindAsync(id)
        is Product product ? Results.Ok(product) : Results.NotFound("Product not found");
}).WithTags("Products");

// UPDATE product
app.MapPut("/products/{id:int}", async ([FromRoute] int id,
                                        [FromBody] Product product,
                                        [FromServices] AppDbContext db) =>
{
    if (product.ProductId != id) return Results.BadRequest();

    var productDB = await db.Products.FindAsync(id);

    if (productDB is null) return Results.NotFound("Product not found.");

    productDB.Name = product.Name;
    productDB.Description = product.Description;
    productDB.Price = product.Price;
    productDB.ImageUrl = product.ImageUrl;
    productDB.PurchaseDate = product.PurchaseDate;
    productDB.Stock = product.Stock;
    productDB.CategoryId = product.CategoryId;

    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithTags("Products");

// REMOVE product
app.MapDelete("/products/{id:int}", async ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null) return Results.NotFound("Product not found.");

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithTags("Products");

app.Run();
