using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.TenantDatabase.Areas.Products.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public string Sku { get; set; }

        public Guid? SchemaId { get; set; }

        public string FormData { get; set; }
    }
}
