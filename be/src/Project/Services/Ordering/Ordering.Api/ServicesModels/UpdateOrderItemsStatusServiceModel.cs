using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderItemsStatusServiceModel
    {
        public Guid OrderId { get; set; }
        public int OrderItemIndex { get; set; }
        public bool IsDone { get; set; }
    }
}
