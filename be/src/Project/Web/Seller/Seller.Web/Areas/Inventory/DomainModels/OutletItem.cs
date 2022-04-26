using System;

namespace Seller.Web.Areas.Inventory.DomainModels
{
    public class OutletItem
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ean { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
