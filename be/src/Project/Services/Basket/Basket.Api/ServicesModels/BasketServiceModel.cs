using System;
using System.Collections.Generic;

namespace Basket.Api.ServicesModels
{
    public class BasketServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemServiceModel> Items { get; set; }
    }
}
