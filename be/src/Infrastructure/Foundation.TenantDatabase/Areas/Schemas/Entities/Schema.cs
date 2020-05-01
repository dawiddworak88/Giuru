using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Schemas.Entities
{
    public class Schema : Entity
    {
        [Required]
        public string Name { get; set; }

        public virtual EntityType EntityType { get; set; }
    }
}
