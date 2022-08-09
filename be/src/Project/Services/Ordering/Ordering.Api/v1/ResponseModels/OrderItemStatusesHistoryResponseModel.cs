using System;
using System.Collections.Generic;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderItemStatusesHistoryResponseModel
    {
        public Guid? OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusesHistoryItemResponseModel> StatusesHistory { get; set; }
    }
}
