using System;

namespace Media.Api.v1.Areas.Media.ResultModels
{
    public class MediaFileServiceModel
    {
        public Guid Id { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}
