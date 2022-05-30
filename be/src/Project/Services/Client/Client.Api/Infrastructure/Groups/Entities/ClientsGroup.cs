using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Groups.Entities
{
    public class ClientsGroup : Entity
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
