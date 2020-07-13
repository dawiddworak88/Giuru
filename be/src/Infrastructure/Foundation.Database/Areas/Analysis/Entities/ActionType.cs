using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Analysis.Entities
{
    public class ActionType : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
