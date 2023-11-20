using System;

namespace Ordering.Api.v1.RequestModels
{
    public class UpdateOrderItemStatusRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderItemStatusId { get; set; }
        public string ExpectedDateOfProductOnStock { get; set; }
    }
}
