using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class SaveBasketApiRequestModel
    {
        public Guid? Id { get; set; }
        public string MoreInfo { get; set; }
        public IEnumerable<BasketItemApiRequestModel> Items { get; set; }
    }
}
