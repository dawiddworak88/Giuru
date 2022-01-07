using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class OrganisationAddress : Entity
    {
        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public Guid OrganisationId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
