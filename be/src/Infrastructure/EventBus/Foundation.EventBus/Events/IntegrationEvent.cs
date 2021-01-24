using System;
using Newtonsoft.Json;

namespace Foundation.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string Username { get; set; }

        [JsonProperty]
        public string Language { get; set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
