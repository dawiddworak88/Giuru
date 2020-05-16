using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class SchemaField : Entity
    {
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public string FieldName { get; set; }

        public string FieldValue { get; set; }
    }
}
