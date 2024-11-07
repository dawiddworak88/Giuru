using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Basket.Api.v1.RequestModels
{
    public class BasketCheckoutRequestModel : RequestModelBase
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; } 
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public Guid? BillingAddressId { get; set; }
        public string BillingCompany { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingRegion { get; set; }
        public string BillingPostCode { get; set; }
        public string BillingCity { get; set; }
        public string BillingStreet { get; set; }
        public string BillingPhoneNumber { get; set; }
        public Guid? BillingCountryId { get; set; }
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
        public string Reason { get; set; }
        public string MoreInfo { get; set; }
        public bool HasCustomOrder { get; set; }
        public bool HasApprovalToSendEmail { get; set; }
        public IEnumerable<Guid> Attachments { get; set; }
    }
}
