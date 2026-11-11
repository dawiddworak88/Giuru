using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class Basket
    {
        public Guid? Id { get; set; }
        public string MoreInfo { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
