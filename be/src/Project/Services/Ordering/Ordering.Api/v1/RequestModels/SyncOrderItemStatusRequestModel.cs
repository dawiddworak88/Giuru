using System;

namespace Ordering.Api.v1.RequestModels
{
    public class SyncOrderItemStatusRequestModel
    {
        public Guid OrderId { get; set; }
        public int OrderItemIndex { get; set; }
        public bool IsDone { get; set; }
    }
}
