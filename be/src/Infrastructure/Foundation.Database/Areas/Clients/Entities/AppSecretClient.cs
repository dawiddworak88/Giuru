using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Clients.Entities
{
    public class AppSecretClient : Entity
    {
        [Required]
        public Guid AppSecretId { get; set; }

        [Required]
        public Guid ClientId { get; set; }
    }
}
