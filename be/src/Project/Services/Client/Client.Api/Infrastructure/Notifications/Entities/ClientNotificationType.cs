using Foundation.GenericRepository.Entities;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationType : Entity
    {
        public virtual ClientNotificationTypeApproval ClientNotificationTypeApproval { get; set; }
    }
}
