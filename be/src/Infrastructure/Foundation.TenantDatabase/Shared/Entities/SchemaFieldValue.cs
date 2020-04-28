using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class SchemaFieldValue : Entity
    {
        [Required]
        public virtual Schema Schema { get; set; }

        [Required]
        public string FieldName { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string TextValue { get; set; }
    }
}
