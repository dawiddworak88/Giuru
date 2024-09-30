using Foundation.GenericRepository.Entities;
using Microsoft.Identity.Client;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotification : Entity
    {
        [Required]
        public Guid ClientId { get; set; }
        public bool IsRead { get; set; }
    }
}
