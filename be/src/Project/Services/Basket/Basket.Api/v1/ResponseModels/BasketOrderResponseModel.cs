using System;
using System.Collections.Generic;

namespace Basket.Api.v1.ResponseModels
{
    public class BasketOrderResponseModel
    {
        public Guid? Id { get; set; }
        public Guid? OwnerId { get; set; }
        public IEnumerable<BasketOrderItemResponseModel> Items { get; set; }
    }
}
