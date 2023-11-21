using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class CheckoutBasketServiceModel : BaseServiceModel
    {
        public Guid? BasketId { get; set; }
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
        public string MoreInfo { get; set; }
        public string IpAddress { get; set; }
        public string ExternalReference { get; set; }
        public IEnumerable<CheckoutBasketItemServiceModel> Items { get; set; }
        public bool HasCustomOrder { get; set; }
        public IEnumerable<Guid> Attachments { get; set; }
    }
}
