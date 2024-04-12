using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class CheckoutBasketRequestModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public string MoreInfo { get; set; }
        public bool HasCustomOrder { get; set; }
        public IEnumerable<AttachmentRequestModel> Attachments { get; set; }
        public IEnumerable<AttributeValueRequestModel> AttributesValues { get; set; }
    }
}
