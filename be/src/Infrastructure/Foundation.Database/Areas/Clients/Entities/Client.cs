using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public Guid ClientSecret { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
