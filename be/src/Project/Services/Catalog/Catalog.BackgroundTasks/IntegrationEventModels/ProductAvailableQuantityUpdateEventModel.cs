using System;

namespace Catalog.BackgroundTasks.IntegrationEventModels
{
    public class ProductAvailableQuantityUpdateEventModel
    {
        public Guid Id { get; set; }
        public double AvailableQuantity { get; set; }
    }
}
