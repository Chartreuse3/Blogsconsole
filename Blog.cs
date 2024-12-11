using System.ComponentModel.DataAnnotations;

public class Blog
{
  public int BlogId { get; set; }
  public string? Name { get; set; }
  [Required]
  public List<Post>? Posts { get; set; }
}