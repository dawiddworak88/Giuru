using System;

namespace Basket.Api.IntegrationEventsModels
{
    public class BasketItemEventModel
    {
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public double TotalQuantity { get; set; }
        public string ExternalReference { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
