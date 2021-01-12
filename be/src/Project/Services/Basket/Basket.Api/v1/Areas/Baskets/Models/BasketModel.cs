using System;
using System.Collections.Generic;

namespace Basket.Api.v1.Areas.Baskets.Models
{
    public class BasketModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemServiceModel> Items { get; set; }
    }
}
