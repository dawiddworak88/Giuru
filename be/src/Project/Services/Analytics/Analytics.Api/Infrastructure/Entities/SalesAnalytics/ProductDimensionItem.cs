using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductDimensionItem : Entity
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public virtual IEnumerable<ProductTranslationDimensionItem> Translations { get; set; }
    }
}
