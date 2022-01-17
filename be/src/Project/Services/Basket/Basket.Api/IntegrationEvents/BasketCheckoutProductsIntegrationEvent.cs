using Basket.Api.IntegrationEventsModels;
using Foundation.EventBus.Events;
using System;
using System.Collections.Generic;

namespace Basket.Api.IntegrationEvents
{
    public class BasketCheckoutProductsIntegrationEvent : IntegrationEvent
    {
       public IEnumerable<BasketCheckoutProductEventModel> Items { get; set; }
    }
}
