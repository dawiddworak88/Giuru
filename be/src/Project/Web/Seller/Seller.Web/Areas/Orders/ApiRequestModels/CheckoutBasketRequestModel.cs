using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class CheckoutBasketRequestModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
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
        public string MoreInfo { get; set; }
        public bool HasApprovalToSendEmail { get; set; }
    }
}
