using System;
using System.Collections.Generic;

namespace Basket.Api.v1.Areas.Baskets.ResultModels
{
    public class BasketResultModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemResultModel> Items { get; set; }
    }
}
