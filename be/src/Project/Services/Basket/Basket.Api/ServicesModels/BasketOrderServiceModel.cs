using System;
using System.Collections.Generic;

namespace Basket.Api.ServicesModels
{
    public class BasketOrderServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? OwnerId { get; set; }
        public IEnumerable<BasketOrderItemServiceModel> Items { get; set; }
    }
}
