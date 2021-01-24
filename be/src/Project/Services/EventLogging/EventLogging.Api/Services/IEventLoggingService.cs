using System;
using System.Threading.Tasks;

namespace EventLogging.Api.Services
{
    public interface IEventLoggingService
    {
        Task LogAsync(Guid? eventId, string eventState, string content, Guid? entityId, string entityType, string oldValue, string newValue, Guid? organisationId, string username, string source, string ipAddress);
    }
}
