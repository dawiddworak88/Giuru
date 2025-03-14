﻿using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderItemViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ProductUrl { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public string ExternalReference { get; set; }
        public string OrderItemStatusName { get; set; }
        public string ExpectedDateOfProductOnStock { get; set; }
        public Guid? OrderItemStatusId { get; set; }
        public string MoreInfo { get; set; }
    }
}
