using EventLogging.Api.Infrastructure;
using EventLogging.Api.Infrastructure.Events.Entities;
using System;
using System.Threading.Tasks;

namespace EventLogging.Api.Services
{
    public class EventLoggingService : IEventLoggingService
    {
        private readonly EventLoggingContext eventLoggingContext;

        public EventLoggingService(EventLoggingContext eventLoggingContext)
        {
            this.eventLoggingContext = eventLoggingContext;
        }

        public async Task LogAsync(
            Guid? eventId, 
            string eventState, 
            string content, 
            Guid? entityId, 
            string entityType, 
            string oldValue, 
            string newValue, 
            Guid? organisationId, 
            string username, 
            string source, 
            string ipAddress)
        {
            var eventLog = new EventLog
            {
                EventId = eventId,
                EventState = eventState,
                Content = content,
                EntityId = entityId,
                EntityType = entityType,
                OldValue = oldValue,
                NewValue = newValue,
                OrganisationId = organisationId,
                Username = username,
                Source = source,
                IpAddress = ipAddress,
                CreatedDate = DateTime.UtcNow
            };

            this.eventLoggingContext.Add(eventLog);

            await this.eventLoggingContext.SaveChangesAsync();
        }
    }
}
