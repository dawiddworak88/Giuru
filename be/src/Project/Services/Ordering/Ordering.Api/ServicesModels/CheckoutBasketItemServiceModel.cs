using System;

namespace Ordering.Api.ServicesModels
{
    public class CheckoutBasketItemServiceModel
    {
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double TotalQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public double StockQuantity { get; set; }
        public string ExternalReference { get; set; }
        public DateTime? ExpectedDeliveryFrom { get; set; }
        public DateTime? ExpectedDeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
