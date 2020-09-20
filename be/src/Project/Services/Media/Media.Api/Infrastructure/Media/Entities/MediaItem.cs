using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Media.Api.Infrastructure.Media.Entities
{
    public class MediaItem : Entity
    {
        public Guid? OrganisationId { get; set; }
        public bool IsProtected { get; set; }
        public virtual IEnumerable<MediaItemVersion> Versions { get; set; }
    }
}
