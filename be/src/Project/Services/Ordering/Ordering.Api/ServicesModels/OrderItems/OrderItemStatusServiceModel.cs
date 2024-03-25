using System;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class OrderItemStatusServiceModel
    {
        public Guid OrderItemStateId { get; set; }
        public Guid OrderItemStatusId { get; set; }
    }
}
