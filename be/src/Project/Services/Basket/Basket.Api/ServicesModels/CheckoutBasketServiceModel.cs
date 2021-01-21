using Foundation.Extensions.Models;
using System;

namespace Basket.Api.ServicesModels
{
    public class CheckoutBasketServiceModel : BaseServiceModel
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
