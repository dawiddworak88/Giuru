using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class SchemaFieldValue : Entity
    {
        [Required]
        public SchemaField SchemaField { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        public string Value { get; set; }

        public int Version { get; set; }
    }
}
