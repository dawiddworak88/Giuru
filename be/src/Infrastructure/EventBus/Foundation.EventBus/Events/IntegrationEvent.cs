using System;
using Newtonsoft.Json;

namespace Foundation.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            EventName = this.GetType().Name;
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate, string eventName)
        {
            Id = id;
            CreationDate = createDate;
            EventName = eventName;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public Guid? OrganisationId { get; set; }

        [JsonProperty]
        public string Username { get; set; }

        [JsonProperty]
        public string Language { get; set; }

        [JsonProperty]
        public string Source { get; set; }

        [JsonProperty]
        public string IpAddress { get; set; }

        [JsonProperty]
        public string EventName { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
