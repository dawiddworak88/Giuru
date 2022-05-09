using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class CheckoutBasketApiRequestModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string MoreInfo { get; set; }
        public bool HasCustomOrder { get; set; }
        public IEnumerable<Guid> Attachments { get; set; }
    }
}
