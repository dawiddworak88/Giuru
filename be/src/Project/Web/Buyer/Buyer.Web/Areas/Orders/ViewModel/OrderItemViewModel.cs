using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderItemViewModel
    {
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
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
        public string ProductAttributes { get; set; }
    }
}
