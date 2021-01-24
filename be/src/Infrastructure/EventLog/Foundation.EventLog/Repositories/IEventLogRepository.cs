using Foundation.EventBus.Events;
using System.Threading.Tasks;

namespace Foundation.EventLog.Repositories
{
    public interface IEventLogRepository
    {
        Task SaveAsync(IntegrationEvent @event, string eventName, string eventState, string source, string ipAddress);
    }
}
