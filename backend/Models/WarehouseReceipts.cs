namespace backend.Models
{
    public class WarehouseReceipts
    {
        public Guid Id { get; set; }
        public Guid? IdWarehouse { get; set; }
        public Guid? IdSupplier { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string? Note { get; set; }

        public virtual Warehouses? IdWarehouseNavigation { get; set; }
        public virtual Suppliers? IdSupplierNavigation { get; set; }
        public virtual ICollection<WarehouseReceiptDetails> ReceiptDetails { get; set; } = new List<WarehouseReceiptDetails>();
    }
}
