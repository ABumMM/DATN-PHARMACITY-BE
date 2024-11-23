namespace backend.Models
{
    public class WarehouseExports
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public DateTime ExportDate { get; set; }
        public string? Note { get; set; }

        public virtual Warehouses Warehouse { get; set; } = null!;
        
        public virtual ICollection<WarehouseExportDetails> ExportDetails { get; set; } = new List<WarehouseExportDetails>();
    }
}
