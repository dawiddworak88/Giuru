using System;

namespace Ordering.Api.ServicesModels.Orders
{
    public class OrderStatusServiceModel
    {
        public Guid Id { get; set; }
        public Guid OrderStateId { get; set; }
        public string Name { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
