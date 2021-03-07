using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.ProductAttributes.Entities
{
    public class ProductAttribute : Entity
    {
        [Required]
        public Guid SellerId { get; set; }

        public int? Order { get; set; }

        public virtual IEnumerable<ProductAttributeItem> ProductAttributeItems { get; set; }

        public virtual IEnumerable<ProductAttributeTranslation> ProductAttributeTranslations { get; set; }
    }
}
