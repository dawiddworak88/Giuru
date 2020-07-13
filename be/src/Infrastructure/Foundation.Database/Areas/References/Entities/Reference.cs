using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.References.Entities
{
    public class Reference : Entity
    {
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public Guid LinkedEntityId { get; set; }
    }
}
