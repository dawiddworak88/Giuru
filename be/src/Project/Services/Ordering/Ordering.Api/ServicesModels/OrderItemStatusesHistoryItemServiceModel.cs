using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderItemStatusesHistoryItemServiceModel
    {
        public Guid OrderStateId { get; set; }
        public Guid OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public string OrderStatusComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
