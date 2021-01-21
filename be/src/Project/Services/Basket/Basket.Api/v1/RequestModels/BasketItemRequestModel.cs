using System;

namespace Basket.Api.v1.RequestModels
{
    public class BasketItemRequestModel
    {
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
