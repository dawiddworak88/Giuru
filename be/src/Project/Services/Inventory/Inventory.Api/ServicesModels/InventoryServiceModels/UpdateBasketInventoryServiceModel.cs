﻿using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class UpdateBasketInventoryServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
        public double BookedQuantity { get; set; }
    }
}
