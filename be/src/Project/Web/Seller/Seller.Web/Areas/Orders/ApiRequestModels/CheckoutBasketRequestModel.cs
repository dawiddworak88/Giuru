using System;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class CheckoutBasketRequestModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
