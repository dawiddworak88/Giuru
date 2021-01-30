using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderItemServiceModel
    {
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public DateTime? ExpectedDeliveryFrom { get; set; }
        public DateTime? ExpectedDeliveryTo { get; set; }
        public string MoreInfo { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
