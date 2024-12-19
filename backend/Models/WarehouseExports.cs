namespace backend.Models
{
    public class WarehouseExports
    {
        public Guid Id { get; set; }
        public Guid? IdWarehouse { get; set; }
        public DateTime? ExportDate { get; set; }
        public string? Note { get; set; }

        public virtual Warehouses? IdWarehouseNavigation { get; set; }
        
        public virtual ICollection<WarehouseExportDetails> ExportDetails { get; set; } = new List<WarehouseExportDetails>();
    }
}
