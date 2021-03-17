using System;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class ProductAttribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
