using Foundation.EventBus.Events;
using System;

namespace Media.Api.IntegrationEvents
{
    public class UpdatedFileIntegrationEvent : IntegrationEvent
    {
        public Guid FileId { get; set; }
        public string Name { get; set; }
    }
}
