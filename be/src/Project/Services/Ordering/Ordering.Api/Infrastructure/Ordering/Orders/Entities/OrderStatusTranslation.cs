using Foundation.GenericRepository.Entities;
using System;

namespace Ordering.Api.Infrastructure.Ordering.Orders.Entities
{
    public class OrderStatusTranslation : EntityTranslation
    {
        public string Name { get; set; }
        public Guid OrderStatusId { get; set; }
    }
}
