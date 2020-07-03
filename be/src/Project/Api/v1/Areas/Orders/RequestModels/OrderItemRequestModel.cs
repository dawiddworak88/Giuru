using System;

namespace Api.v1.Areas.Orders.RequestModels
{
    public class OrderItemRequestModel
    {
        public string Sku { get; set; }
        public int? Quantity { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
