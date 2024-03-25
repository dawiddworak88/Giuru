using System;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class OrderItemStatusChangeServiceModel
    {
        public Guid OrderItemStateId { get; set; }
        public Guid OrderItemStatusId { get; set; }
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
