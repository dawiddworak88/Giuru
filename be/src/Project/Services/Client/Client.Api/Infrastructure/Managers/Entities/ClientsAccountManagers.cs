using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Managers.Entities
{
    public class ClientsAccountManagers : Entity
    {
        [Required]
        public Guid ClientId { get; set; }
        
        [Required]
        public Guid ClientManagerId { get; set; }
    }
}
