using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsMinimalAPI.Context;
using ProductsMinimalAPI.Models;

namespace ProductsMinimalAPI.Endpoints;

public static class ProductsEndpoint
{
    public static void MapProductsEndpoint(this WebApplication app)
    {
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
        }).WithTags("Products").RequireAuthorization(); ;

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
    }
}
