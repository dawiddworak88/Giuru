using System.Collections.Generic;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class ReadProductAttribute
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
