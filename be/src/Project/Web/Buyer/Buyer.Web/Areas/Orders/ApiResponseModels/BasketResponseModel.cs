using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiResponseModels
{
    public class BasketResponseModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemResponseModel> Items { get; set; }
    }
}
