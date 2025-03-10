﻿using System;
using System.Collections.Generic;

namespace Inventory.Api.v1.ResponseModels
{
    public class OutletSumResponseModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ean { get; set; }
        public double? Quantity { get; set; }
        public double? AvailableQuantity { get; set; }
        public IEnumerable<OutletDetailsResponseModel> Details { get; set; }
        
    }
}
