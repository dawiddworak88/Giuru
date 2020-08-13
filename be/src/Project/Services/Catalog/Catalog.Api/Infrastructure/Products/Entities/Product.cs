using Catalog.Api.Infrastructure.Brands.Entities;
using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Products.Entities
{
    public class Product : Entity
    {
        [Required]
        public string Sku { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual IEnumerable<ProductTranslation> Translations { get; set; }
    }
}
