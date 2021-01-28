using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class BasketItem
    {
        public Guid? ProductId { get; set; }
        public double Quantity { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
