using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Basket.Api.v1.RequestModels
{
    public class BasketRequestModel : RequestModelBase
    {
        public Guid? ClientId { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemRequestModel> Items { get; set; }
    }
}
