using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class ProductAttributeServiceModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
