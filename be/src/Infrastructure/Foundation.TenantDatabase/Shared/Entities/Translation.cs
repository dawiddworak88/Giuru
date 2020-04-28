using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class Translation  : Entity
    {
        [Required]
        public virtual Item Item { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
