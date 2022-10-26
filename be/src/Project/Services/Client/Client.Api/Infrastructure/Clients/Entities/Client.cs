using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Language { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public bool IsOrganisationInformationOutdated { get; set; }

        [Required]
        public Guid SellerId { get; set; }
    }
}
