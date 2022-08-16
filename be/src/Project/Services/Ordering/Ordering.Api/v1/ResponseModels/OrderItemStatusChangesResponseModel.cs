using System;
using System.Collections.Generic;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderItemStatusChangesResponseModel
    {
        public Guid? OrderItemId { get; set; }
        public IEnumerable<OrderItemStatusChangeResponseModel> OrderItemStatusChanges { get; set; }
    }
}
