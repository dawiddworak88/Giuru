using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderAttributeOption
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? AttributeId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
