﻿using System;

namespace Ordering.Api.ServicesModels.OrderItems
{
    public class CheckoutBasketItemServiceModel
    {
        public Guid? ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double OutletQuantity { get; set; }
        public double StockQuantity { get; set; }
        public string ExternalReference { get; set; }
        public string MoreInfo { get; set; }
    }
}