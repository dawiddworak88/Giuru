using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class CheckoutBasketRequestModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string MoreInfo { get; set; }
        public bool HasCustomOrder { get; set; }
    }
}
