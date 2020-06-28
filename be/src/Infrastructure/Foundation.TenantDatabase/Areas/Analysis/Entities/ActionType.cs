using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Analysis.Entities
{
    public class ActionType : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
