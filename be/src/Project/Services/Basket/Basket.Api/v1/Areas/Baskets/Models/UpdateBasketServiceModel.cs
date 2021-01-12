using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Basket.Api.v1.Areas.Baskets.Models
{
    public class UpdateBasketServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemServiceModel> Items { get; set; }
    }
}
