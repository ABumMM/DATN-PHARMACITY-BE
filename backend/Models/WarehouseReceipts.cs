namespace backend.Models
{
    public class WarehouseReceipts
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }

        // Navigation properties
        public virtual Warehouses Warehouse { get; set; } = null!;
        public virtual Suppliers Supplier { get; set; } = null!;
        public virtual ICollection<WarehouseReceiptDetails> ReceiptDetails { get; set; } = new List<WarehouseReceiptDetails>();
    }
}
