using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class SalesFact : Entity
    {
        [Required]
        public double Quantity { get; set; }

        [Required]
        public bool IsStock { get; set; }

        [Required]
        public bool IsOutlet { get; set; }

        public virtual IEnumerable<TimeDimension> TimeDimensions { get; set; }
        public virtual IEnumerable<ClientDimension> ClientDimensions { get; set; }
        public virtual IEnumerable<ProductDimension> ProductDimensions { get; set; }
        public virtual IEnumerable<LocationDimension> LocationDimensions { get; set; }
    }
}
