using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.DomainModels
{
    public class MediaItemVersions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<MediaItem> Versions { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
    }
}
