using System;

namespace Seller.Web.Areas.Orders.DomainModels
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public string ExternalReference { get; set; }
        public string MoreInfo { get; set; }
        public Guid? OrderItemStatusId { get; set; }
        public Guid? LastOrderItemStatusChangeId { get; set; }
        public string OrderItemStatusName { get; set; }
        public string OrderItemStatusChangeComment { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
