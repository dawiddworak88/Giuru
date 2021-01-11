using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class Basket
    {
        public Guid? Id { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
