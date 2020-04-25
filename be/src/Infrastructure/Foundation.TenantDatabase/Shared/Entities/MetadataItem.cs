using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class MetadataItem : Entity
    {
        [Required]
        public virtual Item Item { get; set; }

        [Required]
        public string JsonSchema { get; set; }

        public string UiSchema { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
