using Catalog.Api.Infrastructure.Products.Entities;
using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Catalog.Api.Infrastructure.Brands.Entities
{
    public class Brand : Entity
    {
        public virtual IEnumerable<Product> Products { get; set; }
    }
}
