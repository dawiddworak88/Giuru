using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.TenantDatabase.Areas.Orders.Entitites
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid OrderItemStatus { get; set; }
        public Guid? SchemaId { get; set; }
        public string FormData { get; set; }
    }
}
