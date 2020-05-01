using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Areas.Taxonomies.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class SchemaField : Entity
    {
        [Required]
        public Schema Schema { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        public Taxonomy Options { get; set; }

        public string Default { get; set; }

        public string Format { get; set; }

        public int MaxItems { get; set; }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public string Pattern { get; set; }

        public string UiWidget { get; set; }

        public string EmptyValue { get; set; }
    }
}
