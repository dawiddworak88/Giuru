using Foundation.GenericRepository.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Infrastructure.Entities
{
    public class Product : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Sku { get; set; }

        public string Ean { get; set; }
        public virtual IEnumerable<ProductsGroup> Groups { get; set; }
    }
}
