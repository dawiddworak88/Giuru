using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class Schema : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string JsonSchema { get; set; }

        public string UiSchema { get; set; }

        public Guid? EntityTypeId { get; set; }
    }
}
