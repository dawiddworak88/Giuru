using System;

namespace Seller.Web.Areas.Inventory.DomainModels
{
    public class OutletItem : StockItem
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public Guid SellerId { get; set; }
        public double Quantity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ean { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
