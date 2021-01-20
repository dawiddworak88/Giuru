using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Ordering.Orders.Entities
{
    public class Order : Entity
    {
        public Guid? ClientId { get; set; }

        public Guid? SellerId { get; set; }

        public bool IsGuestOrder { get; set; }

        public string ClientCompanyName { get; set; }

        public string ClientCommunicationLanguage { get; set; }

        public string ClientEmail { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string SellerCompanyName { get; set; }

        public string SellerFirstName { get; set; }

        public string SellerLastName { get; set; }

        public string ExternalReference { get; set; }

        public Guid? BillingAddressId { get; set; }

        public Guid? ShippingAddressId { get; set; }

        public string MoreInfo { get; set; }

        [Required]
        public Guid OrderStatusId { get; set; }

        [Required]
        public Guid OrderStateId { get; set; }

        public string Reason { get; set; }

        [Required]
        public string IpAddress { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
