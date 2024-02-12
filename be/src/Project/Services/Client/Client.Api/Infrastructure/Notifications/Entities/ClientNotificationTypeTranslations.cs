using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationTypeTranslations : EntityTranslation
    {
        [Required]
        public string Name { get; set; }
        public Guid? ClientNotificationTypeId { get; set; }
    }
}
