using System;

namespace Foundation.EventLog.ApiRequestModels
{
    public class SaveEventApiRequestModel
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string EventState { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
