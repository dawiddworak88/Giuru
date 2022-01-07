using Foundation.Catalog.Infrastructure.Categories.Entities;
using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.Products.Entities
{
    public class Product : Entity
    {
        public Guid? PrimaryProductId { get; set; }

        [Required]
        public bool IsProtected { get; set; }

        [Required]
        public bool IsNew { get; set; }

        public string Sku { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual IEnumerable<ProductTranslation> Translations { get; set; }
    }
}
