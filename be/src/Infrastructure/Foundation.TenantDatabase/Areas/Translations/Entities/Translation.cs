using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Translations.Entities
{
    public class Translation  : Entity
    {
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
