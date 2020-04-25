using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Shared.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Products.Entities
{
    public class Product : Entity
    {
        [Required]
        public Item Item { get; set; }

        public string Sku { get; set; }

        public virtual IEnumerable<Translation> Translations { get; set; }
    }
}
