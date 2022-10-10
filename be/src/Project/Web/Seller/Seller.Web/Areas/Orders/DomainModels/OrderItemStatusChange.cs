using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusChange
    {
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
