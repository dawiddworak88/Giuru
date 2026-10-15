using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class Basket
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// The client the basket was last saved, and so priced, for. Null for a basket saved before the client was recorded.
        /// </summary>
        public Guid? ClientId { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
