using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; }

    public int CategoryId { get; set; }
    public bool Discontinued { get; set; }
    
    public Category Category { get; set; }
}
