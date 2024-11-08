namespace backend.Models;

public class Products
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
     
    public string? Detail { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public string? PathImg { get; set; }

    public Guid? IdCategory { get; set; }

    public virtual ICollection<Detailorders> Detailorders { get; set; } = new List<Detailorders>();

    public virtual Categories? IdCategoryNavigation { get; set; }

    public virtual Users? IdUserNavigation { get; set; }
}
