using Foundation.GenericRepository.Entities;
using System;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationTranslation : Entity
    {
        public ClientNotification ClientNotificationId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}
