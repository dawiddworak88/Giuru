using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Groups.Entities
{
    public class GroupTranslation : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
