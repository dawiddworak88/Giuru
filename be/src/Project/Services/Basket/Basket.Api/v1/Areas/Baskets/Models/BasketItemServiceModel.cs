using System;

namespace Basket.Api.v1.Areas.Baskets.Models
{
    public class BasketItemServiceModel
    {
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
