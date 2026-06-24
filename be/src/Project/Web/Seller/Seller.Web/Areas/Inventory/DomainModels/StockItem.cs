using System;

namespace Seller.Web.Areas.Inventory.DomainModels
{
    public class StockItem
    {
        public Guid ProductId { get; set; }
        public double AvailableQuantity { get; set; }
    }
}
