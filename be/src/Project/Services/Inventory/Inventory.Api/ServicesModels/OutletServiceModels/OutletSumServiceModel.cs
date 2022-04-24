using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class OutletSumServiceModel
    {
        public Guid OutletId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProductEan { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public IEnumerable<OutletServiceModel> Details { get; set; }
    }
}
