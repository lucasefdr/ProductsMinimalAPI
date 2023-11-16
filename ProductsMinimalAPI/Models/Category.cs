using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsMinimalAPI.Models;

[Table("category")]
public class Category
{
    [Column("categoryid")]
    public int CategoryId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }
    
    // N => 1 
    public ICollection<Product>? Products { get; set; }
}