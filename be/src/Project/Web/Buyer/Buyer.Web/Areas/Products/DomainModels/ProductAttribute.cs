using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.DomainModels
{
    public class ProductAttribute
    {
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
