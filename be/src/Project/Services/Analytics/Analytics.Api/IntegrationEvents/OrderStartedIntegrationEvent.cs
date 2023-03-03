using Foundation.EventBus.Events;
using System.Collections.Generic;
using System;
using Analytics.Api.IntegrationEventsModels;

namespace Analytics.Api.IntegrationEvents
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<OrderItemStartedEventModel> OrderItems { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
