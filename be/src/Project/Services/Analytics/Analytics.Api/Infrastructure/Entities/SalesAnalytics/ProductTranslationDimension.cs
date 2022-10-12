using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ProductTranslationDimension : EntityTranslation
    {
        [Required]
        public Guid ProductDimensionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
