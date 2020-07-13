using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Shared.Entities
{
    public class EntityType : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
