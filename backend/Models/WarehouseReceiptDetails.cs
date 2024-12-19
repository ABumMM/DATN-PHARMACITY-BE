namespace backend.Models
{
    public class WarehouseReceiptDetails
    {
        public Guid Id { get; set; }
        public Guid? IdReceipt { get; set; }
        public Guid? IdProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual WarehouseReceipts? IdReceiptNavigation { get; set; }
        public virtual Products? IdProductNavigation { get; set; }
    }
}
