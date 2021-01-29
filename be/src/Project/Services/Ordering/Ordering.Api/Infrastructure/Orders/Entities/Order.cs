using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class Order : Entity
    {
        public Guid? ClientId { get; set; }

        public Guid? SellerId { get; set; }

        public Guid? BillingAddressId { get; set; }

        public Guid? ShippingAddressId { get; set; }

        public string MoreInfo { get; set; }

        [Required]
        public Guid OrderStatusId { get; set; }

        [Required]
        public Guid OrderStateId { get; set; }

        public string Reason { get; set; }

        public string IpAddress { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
