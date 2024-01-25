using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationType : Entity
    {
        public virtual ClientNotificationTypeApproval ClientNotificationTypeApproval { get; set; }
        public IEnumerable<ClientNotificationTypeTranslations> Translations { get; set; }
    }
}
