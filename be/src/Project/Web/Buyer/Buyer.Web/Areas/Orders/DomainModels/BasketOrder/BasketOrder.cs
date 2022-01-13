using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.DomainModels.BasketOrder
{
    public class BasketOrder
    {
        public Guid? Id { get; set; }
        public Guid OwnerId { get; set; }
        public IEnumerable<BasketItems> Items { get; set; }
    }
}
