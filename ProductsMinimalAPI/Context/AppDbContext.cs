using Microsoft.EntityFrameworkCore;
using ProductsMinimalAPI.Models;

namespace ProductsMinimalAPI.Context;

public class AppDbContext(DbContextOptions opts) : DbContext(opts)
{
    DbSet<Category>? Categories { get; set; }
    DbSet<Product>? Products { get; set; }
}
