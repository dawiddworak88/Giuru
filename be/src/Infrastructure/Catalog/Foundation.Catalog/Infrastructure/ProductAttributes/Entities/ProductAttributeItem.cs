using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Catalog.Infrastructure.ProductAttributes.Entities
{
    public class ProductAttributeItem : Entity
    {
        [Required]
        public Guid ProductAttributeId { get; set; }

        [Required]
        public Guid SellerId { get; set; }

        public int? Order { get; set; }

        public virtual IEnumerable<ProductAttributeItemTranslation> ProductAttributeItemTranslations { get; set; }
    }
}
