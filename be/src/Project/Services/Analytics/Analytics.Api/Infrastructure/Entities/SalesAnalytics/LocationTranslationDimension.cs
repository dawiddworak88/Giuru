using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class LocationTranslationDimension : EntityTranslation
    {
        [Required]
        public Guid LocationDimensionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
