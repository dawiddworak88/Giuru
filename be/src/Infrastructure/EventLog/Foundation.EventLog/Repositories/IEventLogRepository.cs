using Foundation.EventBus.Events;
using Foundation.GenericRepository.EntitiesLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.EventLog.Repositories
{
    public interface IEventLogRepository
    {
        Task SaveAsync(IntegrationEvent @event, string eventState);
        Task SaveAsync(
            Guid entityId,
            string entityType,
            IEnumerable<EntityLogProperty> modifiedProperties,
            Guid organisationId,
            string username);
    }
}
