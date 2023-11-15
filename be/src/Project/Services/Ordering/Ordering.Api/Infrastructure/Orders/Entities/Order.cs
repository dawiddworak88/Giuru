using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class Order : Entity
    {
        public Guid? ClientId { get; set; }

        public string ClientName { get; set; }

        public Guid? SellerId { get; set; }

        public Guid? BillingAddressId { get; set; }

        public string BillingCompany { get; set; }

        public string BillingFirstName { get; set; }

        public string BillingLastName { get; set; }

        public string BillingRegion { get; set; }

        public string BillingPostCode { get; set; }

        public string BillingCity { get; set; }

        public string BillingStreet { get; set; }

        public string BillingPhonePrefix { get; set; }

        public string BillingPhone { get; set; }

        public string BillingCountryCode { get; set; }

        public Guid? ShippingAddressId { get; set; }

        public string ShippingCompany { get; set; }

        public string ShippingFirstName { get; set; }

        public string ShippingLastName { get; set; }

        public string ShippingRegion { get; set; }

        public string ShippingPostCode { get; set; }

        public string ShippingCity { get; set; }

        public string ShippingStreet { get; set; }

        public string ShippingPhoneNumber { get; set; }

        public Guid? ShippingCountryId { get; set; }

        public string ExternalReference { get; set; }

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
