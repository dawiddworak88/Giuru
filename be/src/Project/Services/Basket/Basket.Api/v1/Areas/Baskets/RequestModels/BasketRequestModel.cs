using Foundation.ApiExtensions.Models.Request;
using System.Collections.Generic;

namespace Basket.Api.v1.Areas.Baskets.RequestModels
{
    public class BasketRequestModel : RequestModelBase
    {
        public IEnumerable<BasketItemRequestModel> Items { get; set; }
    }
}
