using Foundation.ApiExtensions.Models.Request;
using System.Collections.Generic;

namespace Basket.Api.v1.RequestModels
{
    public class BasketRequestModel : RequestModelBase
    {
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemRequestModel> Items { get; set; }
    }
}
