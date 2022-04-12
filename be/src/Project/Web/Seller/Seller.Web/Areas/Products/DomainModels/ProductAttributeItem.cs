using System;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class ProductAttributeItem
    {
        public Guid Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
