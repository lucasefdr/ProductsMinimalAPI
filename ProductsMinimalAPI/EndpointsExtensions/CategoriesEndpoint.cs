using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsMinimalAPI.Context;
using ProductsMinimalAPI.Models;

namespace ProductsMinimalAPI.Endpoints;

public static class CategoriesEndpoint
{
    // Categories endpoint with extension method
    public static void MapCategoriesEndpoint(this WebApplication app)
    {
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
        }).WithTags("Categories").RequireAuthorization();

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
    }
}
