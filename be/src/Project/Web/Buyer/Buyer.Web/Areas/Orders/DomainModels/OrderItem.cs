using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public string ExternalReference { get; set; }
        public string ProductAttributes { get; set; }
        public string MoreInfo { get; set; }
        public string OrderStatusName { get; set; }
        public string OrderStatusComment { get; set; }
        public DateTime? ExpectedDeliveryFrom { get; set; }
        public DateTime? ExpectedDeliveryTo { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
