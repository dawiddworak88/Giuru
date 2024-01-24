using Foundation.GenericRepository.Entities;
using Microsoft.Identity.Client;
using System;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotification : Entity
    {
        public Guid ClientId { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTimeSent { get; set; }
    }
}
