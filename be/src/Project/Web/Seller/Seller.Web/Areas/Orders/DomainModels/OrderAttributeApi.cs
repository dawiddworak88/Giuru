using System.Collections.Generic;
using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderAttributeApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOrderItemAttribute { get; set; }
        public IEnumerable<OrderAttributeOptionItem> Options { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
