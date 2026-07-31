using System;
using System.Collections.Generic;

namespace Inventory.Api.DomainModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public IEnumerable<ProductAttribute> ProductAttributes { get; set; }
    }
}
