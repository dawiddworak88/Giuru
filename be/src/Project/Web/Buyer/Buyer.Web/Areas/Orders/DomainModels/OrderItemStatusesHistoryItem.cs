using System;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusesHistoryItem
    {
        public string OrderStatusName { get; set; }
        public string OrderStatusComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
