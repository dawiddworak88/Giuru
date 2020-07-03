using System;

namespace Feature.Order.Models
{
    public class OrderItemModel
    {
        public string Sku { get; set; }
        public int? Quantity { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
