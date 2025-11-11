using System;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class BasketItem
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public string ExternalReference { get; set; }
        public string MoreInfo { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string IsStock { get; set; }
    }
}
