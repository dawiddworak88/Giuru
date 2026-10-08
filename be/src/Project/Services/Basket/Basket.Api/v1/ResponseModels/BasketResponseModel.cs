using System;
using System.Collections.Generic;

namespace Basket.Api.v1.ResponseModels
{
    public class BasketResponseModel
    {
        public Guid? Id { get; set; }
        public string DiscountCode { get; set; }
        public Guid? DiscountCodeClientId { get; set; }
        public Guid? BasketVersion { get; set; }
        public IEnumerable<BasketItemResponseModel> Items { get; set; }
    }
}
