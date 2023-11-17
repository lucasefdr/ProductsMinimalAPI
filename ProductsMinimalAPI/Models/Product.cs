using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsMinimalAPI.Models;

[Table("product")]
public class Product
{
    [Column("productid")]
    public int ProductId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("imageurl")]
    public string ImageUrl { get; set; }

    [Column("purchasedate")]
    public DateTime PurchaseDate { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("categoryid")]
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
}
