namespace backend.Models
{
    public class WarehouseExportDetails
    {
        public Guid Id { get; set; }
        public Guid IdExport { get; set; }
        public Guid IdProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual WarehouseExports? IdExportNavigation { get; set; }
        
        public virtual Products? IdProductNavigation { get; set; }
    }
}
