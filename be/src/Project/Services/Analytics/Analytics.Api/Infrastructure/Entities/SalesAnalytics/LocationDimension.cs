using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class LocationDimension : Entity
    {
        [Required]
        public string Country { get; set; }
    }
}
