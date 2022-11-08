using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class LocationDimension : Entity
    {
        [Required]
        public Guid SalesFactId { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
