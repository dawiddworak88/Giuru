using System;

namespace Catalog.Api.v1.Areas.Schemas.Models
{
    public class OrderItemModel
    {
        public string Sku { get; set; }
        public int? Quantity { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
