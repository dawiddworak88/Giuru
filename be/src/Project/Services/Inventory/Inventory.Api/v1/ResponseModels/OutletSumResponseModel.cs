using System;
using System.Collections.Generic;

namespace Inventory.Api.v1.ResponseModels
{
    public class OutletSumResponseModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public IEnumerable<OutletDetailsResponseModel> Details { get; set; }
        
    }
}
