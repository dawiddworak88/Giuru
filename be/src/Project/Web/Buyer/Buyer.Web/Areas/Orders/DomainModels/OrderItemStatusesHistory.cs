using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusesHistory
    {
        public Guid OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusesHistoryItem> StatusesHistory { get; set; }
    }
}
