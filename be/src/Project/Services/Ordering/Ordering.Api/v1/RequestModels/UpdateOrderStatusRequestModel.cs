using System;

namespace Ordering.Api.v1.RequestModels
{
    public class UpdateOrderStatusRequestModel
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderStatusId { get; set; }
    }
}
