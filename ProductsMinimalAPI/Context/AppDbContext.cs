using Microsoft.EntityFrameworkCore;
using ProductsMinimalAPI.Models;

namespace ProductsMinimalAPI.Context;

public class AppDbContext(DbContextOptions opts) : DbContext(opts)
{
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Category
        modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Category>().Property(c => c.Description).HasMaxLength(150).IsRequired();

        // Product
        modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.ImageUrl).HasMaxLength(150);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);

        // Relationship
        modelBuilder.Entity<Product>().HasOne<Category>(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }

    internal Task FindAsync(int id)
    {
        throw new NotImplementedException();
    }
}
