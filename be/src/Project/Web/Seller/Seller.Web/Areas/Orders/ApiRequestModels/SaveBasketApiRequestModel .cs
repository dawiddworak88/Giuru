using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class SaveBasketApiRequestModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemApiRequestModel> Items { get; set; }
    }
}
