using Foundation.GenericRepository.Entities;
using System;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductTranslationDimensionItem : EntityTranslation
    {
        public Guid ProductDimensionId { get; set; }
        public string Name { get; set; }
    }
}
