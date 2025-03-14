﻿using Foundation.ApiExtensions.Models.Response;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class Order : BaseResponseModel
    {
        public Guid OrderStatusId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
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
        public string ShippingPhonePrefix { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingCountryCode { get; set; }
        public string MoreInfo { get; set; }
        public string OrderStatusName { get; set; }
        public Guid OrderStateId { get; set; }
        public string Reason { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<Guid> Attachments { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
