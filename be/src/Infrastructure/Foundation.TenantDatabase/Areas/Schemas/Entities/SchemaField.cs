using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Areas.Taxonomies.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class SchemaField : Entity
    {
        [Required]
        public virtual Schema Schema { get; set; }

        [Required]
        public string Title { get; set; }

        public string LabelKey { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        public string IsRequiredValidationMessageKey { get; set; }

        public virtual Taxonomy Options { get; set; }

        public string Default { get; set; }

        public string Format { get; set; }

        public int? MaxItems { get; set; }

        public string MaxItemsValidationMessageKey { get; set; }

        public int? MinLength { get; set; }

        public string MinLengthValidationMessageKey { get; set; }

        public int? MaxLength { get; set; }

        public string MaxLengthValidationMessageKey { get; set; }

        public string Pattern { get; set; }

        public string PatternValidationMessageKey { get; set; }

        public string UiWidget { get; set; }

        public string EmptyValue { get; set; }

        [Required]
        public bool IsSearchable { get; set; }

        [Required]
        public bool IsFilterable { get; set; }

        [Required]
        public bool IsSortable { get; set; }

        [Required]
        public bool IsFacetable { get; set; }
    }
}
