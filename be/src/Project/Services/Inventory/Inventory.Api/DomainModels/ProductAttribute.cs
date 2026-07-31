using System.Collections.Generic;

namespace Inventory.Api.DomainModels
{
    public class ProductAttribute
    {
        public string Key { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
