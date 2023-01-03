using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductDimension : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string Sku { get; set; }

        [Required]
        public string Ean { get; set; }

        public virtual IEnumerable<ProductTranslationDimension> Translations { get; set; }
    }
}
