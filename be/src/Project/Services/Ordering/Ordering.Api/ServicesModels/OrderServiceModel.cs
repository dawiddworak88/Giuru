using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class OrderServiceModel
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public string MoreInfo { get; set; }
        public Guid OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public Guid OrderStateId { get; set; }
        public string Reason { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public IEnumerable<OrderItemServiceModel> OrderItems { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
