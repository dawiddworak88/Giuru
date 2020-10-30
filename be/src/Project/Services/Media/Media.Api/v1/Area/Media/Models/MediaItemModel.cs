using System;

namespace Media.Api.v1.Area.Media.Models
{
    public class MediaItemModel
    {
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public string Folder { get; set; }
    }
}
