using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class CategorySchemas
    {
        public Guid? CategotyId { get; set; }
        public IEnumerable<CategorySchema> Schemas { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
