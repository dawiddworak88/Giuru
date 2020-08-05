using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Clients.Entities
{
    public class AddressClient : Entity
    {
        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
