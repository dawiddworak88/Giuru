using System;

namespace Media.Api.ServicesModels
{
    public class MediaFileItemServiceModel
    {
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public string Folder { get; set; }
    }
}
