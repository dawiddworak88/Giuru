using Catalog.Api.Infrastructure.Media.Enums;
using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Media.Entities
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
