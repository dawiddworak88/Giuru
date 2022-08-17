using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusChanges
    {
        public Guid OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusChange> StatusChanges { get; set; }
    }
}
