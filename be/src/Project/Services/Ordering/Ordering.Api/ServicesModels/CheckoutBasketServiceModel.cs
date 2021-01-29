using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class CheckoutBasketServiceModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public string MoreInfo { get; set; }
        public string IpAddress { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public IEnumerable<CheckoutBasketItemServiceModel> Items { get; set; }

    }
}
