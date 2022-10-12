using Foundation.GenericRepository.Entities;
using System;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductTranslationDimension : EntityTranslation
    {
        public Guid ProductDimensionId { get; set; }
        public string Name { get; set; }
    }
}
