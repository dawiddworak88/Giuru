using System;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusChange
    {
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
