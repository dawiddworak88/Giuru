using Foundation.GenericRepository.Entities;
using System;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationTypeTranslations : Entity
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public Guid? CLientNotoficationTypeId { get; set; }
    }
}
