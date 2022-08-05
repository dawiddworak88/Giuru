using System;

namespace Ordering.Api.v1.ResponseModels
{
    public class OrderItemResponseModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double Quantity { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
        public string ProductSku { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public string ExternalReference { get; set; }
        public string OrderStatusName { get; set; }
        public Guid OrderStatusId { get; set; }
        public Guid OrderStateId { get; set; }
        public string OrderStatusComment { get; set; }
        public Guid LastOrderItemStatusChangeId { get; set; }
        public DateTime? ExpectedDeliveryFrom { get; set; }
        public DateTime? ExpectedDeliveryTo { get; set; }
        public string MoreInfo { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
