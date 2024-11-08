namespace backend.Models;

public partial class Detailorders
{
    public Guid Id { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Guid? IdOrder { get; set; }

    public Guid? IdProduct { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Orders? IdOrderNavigation { get; set; }

    public virtual Products? IdProductNavigation { get; set; }
}
