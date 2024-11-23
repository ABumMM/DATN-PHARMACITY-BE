namespace backend.Models
{
    public class WarehouseExportDetails
    {
        public Guid Id { get; set; }
        public Guid ExportId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual WarehouseExports Export { get; set; } = null!;
        
        public virtual Products Product { get; set; } = null!;
    }
}
