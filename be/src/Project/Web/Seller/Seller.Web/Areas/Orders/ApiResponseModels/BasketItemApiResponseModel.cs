using System;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketItemApiResponseModel
    {
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
