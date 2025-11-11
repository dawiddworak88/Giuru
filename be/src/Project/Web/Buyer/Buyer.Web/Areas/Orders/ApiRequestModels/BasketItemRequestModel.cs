using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class BasketItemRequestModel
    {
        public Guid? ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ImageSrc { get; set; }
        public Guid? ImageId { get; set; }
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
