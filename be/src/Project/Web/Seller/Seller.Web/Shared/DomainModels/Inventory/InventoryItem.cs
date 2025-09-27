using System;

namespace Seller.Web.Shared.DomainModels.Inventory
{
    public class InventoryItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public double? Quantity { get; set; }
        public double? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
