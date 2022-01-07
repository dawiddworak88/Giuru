using System.Collections.Generic;

namespace Catalog.Api.v1.Products.ResponseModels
{
    public class ProductAttributeValuesResponseModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
