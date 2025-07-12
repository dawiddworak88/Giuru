using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Price { get; set; }

        [Column(TypeName = "nvarchar(3)")]
        public string? Currency { get; set; }
    }
}
