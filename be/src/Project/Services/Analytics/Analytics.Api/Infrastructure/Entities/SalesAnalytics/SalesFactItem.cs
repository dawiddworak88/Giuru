using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class SalesFactItem : Entity
    {
        public Guid TimeDimensionId { get; set; }
        public Guid ClientDimensionId { get; set; }
        public Guid ProductDimensionId { get; set; }
        public double Quantity { get; set; }
        public bool IsStock { get; set; }
        public bool IsOutlet { get; set; }
        public virtual IEnumerable<TimeDimensionItem> TimeDimensions { get; set; }
        public virtual IEnumerable<ClientDimensionItem> ClientDimensions { get; set; }
        public virtual IEnumerable<ProductDimensionItem> ProductDimensions { get; set; }
    }
}
