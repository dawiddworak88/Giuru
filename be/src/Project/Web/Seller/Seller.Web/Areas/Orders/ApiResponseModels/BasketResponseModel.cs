using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class BasketResponseModel
    {
        public Guid? Id { get; set; }
        public string DiscountCode { get; set; }

        /// <summary>Set when this save dropped a stored code that can no longer be applied. Tells the user why.</summary>
        public string DiscountCodeRemovedMessage { get; set; }
        public IEnumerable<BasketItemResponseModel> Items { get; set; }
    }
}
