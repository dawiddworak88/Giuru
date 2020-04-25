using Foundation.GenericRepository.Entities;
using Foundation.TenantDatabase.Shared.Entities;

namespace Foundation.TenantDatabase.Areas.Media.Entities
{
    public class LinkMediaItem : Entity
    {
        public virtual Item Item { get; set; }

        public virtual MediaItem MediaItem { get; set; }
    }
}
