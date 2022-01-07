using Foundation.GenericRepository.Entities;
using Identity.Api.Infrastructure.Organisations.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class Connection : Entity
    {
        /// <summary>
        /// Organisation Id
        /// </summary>
        [Required]
        public Guid ConnectedFrom { get; set; }

        /// <summary>
        /// Organisation Id
        /// </summary>
        [Required]
        public Guid ConnectedTo { get; set; }

        [Required]
        public InvitationStatus InvitationStatus { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}
