namespace backend.Models;

public class Orders
{
    public Guid Id { get; set; }

    public int? Status { get; set; }

    public decimal? Total { get; set; } 

    public DateTime CreateAt { get; set; }

    public Guid? IdUser { get; set; }

    public Guid? IdPromotion { get; set; }
    
    public virtual Promotions? Promotion { get; set; }

    public virtual ICollection<Detailorders> Detailorders { get; set; } = new List<Detailorders>();
}
