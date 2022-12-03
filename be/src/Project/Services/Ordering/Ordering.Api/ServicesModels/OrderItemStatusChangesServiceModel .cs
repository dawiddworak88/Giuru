using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class OrderItemStatusChangesServiceModel
    {
        public Guid? OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusChangeServiceModel> OrderItemStatusChanges { get; set; }
    }
}
