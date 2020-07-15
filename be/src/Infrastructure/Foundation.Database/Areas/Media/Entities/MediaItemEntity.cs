using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Media.Entities
{
    public class MediaItemEntity : Entity
    {
        [Required]
        public Guid MediaItemId { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        MediaItemType MediaItemType { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
