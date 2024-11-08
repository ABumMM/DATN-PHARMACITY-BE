namespace backend.Models;

public class Promotions
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal DiscountPercentage { get; set; }

    public int Quantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool IsActive { get; set; }
}
