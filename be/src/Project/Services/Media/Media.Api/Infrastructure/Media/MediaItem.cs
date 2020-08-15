using Foundation.GenericRepository.Entities;
using System.Collections.Generic;

namespace Media.Api.Infrastructure.Media
{
    public class MediaItem : Entity
    {
        public bool IsProtected { get; set; }
        public virtual IEnumerable<MediaItemVersion> Versions { get; set; }
    }
}
