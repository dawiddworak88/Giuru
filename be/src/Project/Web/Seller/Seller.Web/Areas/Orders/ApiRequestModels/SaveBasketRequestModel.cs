using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class SaveBasketRequestModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemRequestModel> Items { get; set; }
    }
}
