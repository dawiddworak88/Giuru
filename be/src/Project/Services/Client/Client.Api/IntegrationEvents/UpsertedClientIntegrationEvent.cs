using Foundation.EventBus.Events;
using System;

namespace Client.Api.IntegrationEvents
{
    public class UpsertedClientIntegrationEvent : IntegrationEvent
    {
        public Guid ClientId { get; set; }
    }
}