using System;

namespace Ordering.Api.v1.RequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid? OrderItemId { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
