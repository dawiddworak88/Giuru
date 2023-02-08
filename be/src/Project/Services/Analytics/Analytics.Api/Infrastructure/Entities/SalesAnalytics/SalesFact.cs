using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class SalesFact : Entity
    {
        [Required]
        public Guid ProductDimensionId { get; set; }

        [Required]
        public Guid ClientDimensionId { get; set; }

        [Required]
        public Guid TimeDimensionId { get; set; }

        public Guid? LocationDimensionId { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public bool IsStock { get; set; }

        [Required]
        public bool IsOutlet { get; set; }
    }
}
