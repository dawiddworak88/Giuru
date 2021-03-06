using System;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketItemResponseModel
    {
        public Guid? ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public double Quantity { get; set; }
        public string ExternalReference { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
