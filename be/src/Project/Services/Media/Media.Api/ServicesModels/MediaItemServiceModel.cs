using System;

namespace Media.Api.v1.Areas.Media.ResultModels
{
    public class MediaItemServiceModel
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
        public string MetaData { get; set; }
        public long Size { get; set; }
        public bool IsProtected { get; set; }
        public Guid? MediaItemId { get; set; }
        public Guid? MediaItemVersionId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
