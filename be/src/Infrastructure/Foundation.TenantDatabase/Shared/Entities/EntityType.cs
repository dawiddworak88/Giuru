using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class EntityType : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
