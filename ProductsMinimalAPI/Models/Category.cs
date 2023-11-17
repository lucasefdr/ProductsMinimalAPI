using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}