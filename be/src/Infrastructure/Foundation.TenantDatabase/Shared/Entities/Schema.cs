using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class Schema : Entity
    {
        [Required]
        public virtual Item Item { get; set; }

        public virtual Item EntityType { get; set; }

        [Required]
        public string JsonSchema { get; set; }

        public string UiSchema { get; set; }
    }
}
