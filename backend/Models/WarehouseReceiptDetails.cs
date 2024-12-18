namespace backend.Models
{
    public class WarehouseReceiptDetails
    {
        public Guid Id { get; set; }
        public Guid ReceiptId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Navigation properties
        public virtual WarehouseReceipts Receipt { get; set; } = null!;
        public virtual Products Product { get; set; } = null!;
    }
}
