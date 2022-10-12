using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductDimension : Entity
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public virtual IEnumerable<ProductTranslationDimension> Translations { get; set; }
    }
}
