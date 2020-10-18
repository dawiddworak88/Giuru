using Foundation.GenericRepository.Entities;
using System;
using System.Collections.Generic;

namespace Media.Api.Infrastructure.Media.Entities
{
    public class MediaItemVersion : Entity
    {
        public long Size { get; set; }
        public string Folder { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public int Version { get; set; }
        public string Checksum { get; set; }
        public string CreatedBy { get; set; }
        public Guid MediaItemId { get; set; }
        public virtual IEnumerable<MediaItemTranslation> Translations { get; set; }
    }
}
