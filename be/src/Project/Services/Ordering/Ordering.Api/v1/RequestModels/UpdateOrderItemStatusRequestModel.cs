using System;

namespace Ordering.Api.v1.RequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderStatusId { get; set; }
        public string OrderStatusComment { get; set; }
    }
}
