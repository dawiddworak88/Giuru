using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ApiResponseModels
{
    public class BasketResponseModel
    {
        public Guid? Id { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemResponseModel> Items { get; set; }

        /// <summary>Set when the save changed what was asked for - it dropped a discount code that is no longer valid.</summary>
        public string Message { get; set; }
    }
}
