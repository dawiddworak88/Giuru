using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderFile
    {
        public Guid Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
