namespace backend.Models;
 
public partial class Categories
{
    public Guid Id { get; set; }

    public string? Slug { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
