using Foundation.EventBus.Events;
using Ordering.Api.IntegrationEventsModels;
using System;
using System.Collections.Generic;

namespace Ordering.Api.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid? BasketId { get; set; }
        public Guid? ClientId { get; set; }
        public IEnumerable<OrderItemStartedEventModel> OrderItems { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
