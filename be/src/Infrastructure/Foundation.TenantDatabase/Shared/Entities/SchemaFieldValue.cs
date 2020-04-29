using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class SchemaFieldValue : Entity
    {
        [Required]
        public virtual Item Item { get; set; }

        [Required]
        public string FieldName { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
