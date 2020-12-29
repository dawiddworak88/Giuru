using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
