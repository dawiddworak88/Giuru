using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.GenericRepository.Entities
{
    public class EntityMedia : Entity
    {
        public Guid MediaId { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
