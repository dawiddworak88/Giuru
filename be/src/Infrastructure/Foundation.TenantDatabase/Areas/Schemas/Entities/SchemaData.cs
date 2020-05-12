using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class SchemaData : Entity
    {
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public string FormData { get; set; }
    }
}
