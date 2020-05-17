using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Media.Entities
{
    public class LinkMediaItem : Entity
    {
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public virtual MediaItem MediaItem { get; set; }
    }
}
