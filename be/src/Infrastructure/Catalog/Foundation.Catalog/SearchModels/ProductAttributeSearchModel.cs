using System.Collections.Generic;

namespace Foundation.Catalog.SearchModels
{
    public class ProductAttributeSearchModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductAttributeValueSearchModel> Values { get; set; }
    }
}
