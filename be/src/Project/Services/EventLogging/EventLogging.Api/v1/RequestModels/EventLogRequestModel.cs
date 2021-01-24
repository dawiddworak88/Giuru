using System;

namespace EventLogging.Api.v1.RequestModels
{
    public class EventLogRequestModel
    {
        public Guid? EventId { get; set; }

        public string EventState { get; set; }

        public string EntityType { get; set; }

        public Guid? EntityId { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string Content { get; set; }

        public Guid? OrganisationId { get; set; }

        public string Username { get; set; }

        public string Source { get; set; }

        public string IpAddress { get; set; }
    }
}
