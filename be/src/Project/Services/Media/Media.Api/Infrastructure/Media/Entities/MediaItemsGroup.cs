using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Media.Api.Infrastructure.Media.Entities
{
    public class MediaItemsGroup : Entity
    {
        [Required]
        public Guid MediaItemId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
