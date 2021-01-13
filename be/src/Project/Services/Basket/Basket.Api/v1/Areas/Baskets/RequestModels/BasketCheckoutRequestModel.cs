using Foundation.ApiExtensions.Models.Request;
using System;

namespace Basket.Api.v1.Areas.Baskets.RequestModels
{
    public class BasketCheckoutRequestModel : RequestModelBase
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
