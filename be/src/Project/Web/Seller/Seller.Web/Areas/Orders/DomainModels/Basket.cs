using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class Basket
    {
        public Guid? Id { get; set; }
        public string DiscountCode { get; set; }
        public Guid? DiscountCodeClientId { get; set; }
        public Guid? BasketVersion { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
