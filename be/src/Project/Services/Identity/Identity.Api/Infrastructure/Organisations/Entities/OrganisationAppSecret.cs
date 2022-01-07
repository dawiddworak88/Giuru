using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class OrganisationAppSecret : Entity
    {
        [Required]
        public Guid OrganisationId { get; set; }

        public string AppSecret { get; set; }

        public string CreatedBy { get; set; }
    }
}
