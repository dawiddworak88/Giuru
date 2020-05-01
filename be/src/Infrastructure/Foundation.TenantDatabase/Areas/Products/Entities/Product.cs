using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Areas.Schemas.Entities;

namespace Foundation.TenantDatabase.Areas.Products.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public virtual Schema Schema { get; set; }

        public string Sku { get; set; }
    }
}
