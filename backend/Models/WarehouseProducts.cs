﻿namespace backend.Models
{
    public class WarehouseProducts
    {
        public Guid Id { get; set; }
        public Guid IdWarehouse { get; set; }
        public Guid IdProduct { get; set; }
        public Guid IdSupplier { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual Warehouses? IdWarehouseNavigation { get; set; }
        public virtual Products? IdProductNavigation { get; set; }
        public virtual Suppliers? IdSupplierNavigation { get; set; }
    }
}
