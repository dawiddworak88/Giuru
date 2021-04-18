using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Ordering.Api.Infrastructure.Orders.Entities
{
    public class OrderStatus : Entity
    {
        public Guid OrderStateId { get; set; }
        public int? Order { get; set; }
        public virtual IEnumerable<OrderStatusTranslation> Translations { get; set; }
    }
}
