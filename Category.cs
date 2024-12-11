using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; }

    public string Description { get; set; }
    public List<Product> Products { get; set; }
}
