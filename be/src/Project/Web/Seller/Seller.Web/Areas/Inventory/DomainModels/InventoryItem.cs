﻿using System;

namespace Seller.Web.Areas.Inventory.DomainModels
{
    public class InventoryItem
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public Guid SellerId { get; set; }
        public double Quantity { get; set; }
        public string Ean { get; set; }
        public int? RestockableInDays { get; set; }
        public double AvailableQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
