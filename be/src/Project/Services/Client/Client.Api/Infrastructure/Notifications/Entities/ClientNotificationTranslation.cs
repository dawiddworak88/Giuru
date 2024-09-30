using Foundation.GenericRepository.Entities;
using System;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationTranslation : EntityTranslation
    {
        public Guid? ClientNotificationId { get; set; }
        public string Message { get; set; }
    }
}
