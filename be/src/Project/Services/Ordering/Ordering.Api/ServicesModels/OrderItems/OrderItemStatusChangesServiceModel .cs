using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class OrderItemStatusChangesServiceModel
    {
        public Guid? OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusChangeServiceModel> OrderItemStatusChanges { get; set; }
    }
}
