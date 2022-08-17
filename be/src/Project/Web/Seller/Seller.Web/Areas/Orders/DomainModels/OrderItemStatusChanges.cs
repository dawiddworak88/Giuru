using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderItemStatusChanges
    {
        public Guid OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusChange> StatusChanges { get; set; }
    }
}
