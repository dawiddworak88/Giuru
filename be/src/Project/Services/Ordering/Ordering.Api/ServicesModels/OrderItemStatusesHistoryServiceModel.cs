using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class OrderItemStatusesHistoryServiceModel
    {
        public Guid? OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusesHistoryItemServiceModel> StatusesHistory { get; set; }
    }
}
