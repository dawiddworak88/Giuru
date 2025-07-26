using System;

namespace Catalog.BackgroundTasks.IntegrationEventModels
{
    public class ProductAvailableQuantityUpdateEventModel
    {
        public string ProductSku { get; set; }
        public double AvailableQuantity { get; set; }
    }
}
