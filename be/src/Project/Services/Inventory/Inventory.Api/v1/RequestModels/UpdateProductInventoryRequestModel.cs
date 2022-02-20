using System;

namespace Inventory.Api.v1.RequestModels
{
    public class UpdateProductInventoryRequestModel
    {
        public string WarehouseName { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
