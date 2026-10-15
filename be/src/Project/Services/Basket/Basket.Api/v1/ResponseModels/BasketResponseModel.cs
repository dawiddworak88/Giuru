using System;
using System.Collections.Generic;

namespace Basket.Api.v1.ResponseModels
{
    public class BasketResponseModel
    {
        public Guid? Id { get; set; }
        public Guid? ClientId { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemResponseModel> Items { get; set; }
    }
}
