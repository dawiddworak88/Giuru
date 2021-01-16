using Foundation.Extensions.Models;
using System;

namespace Basket.Api.v1.Areas.Baskets.Models
{
    public class CheckoutBasketServiceModel : BaseServiceModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
