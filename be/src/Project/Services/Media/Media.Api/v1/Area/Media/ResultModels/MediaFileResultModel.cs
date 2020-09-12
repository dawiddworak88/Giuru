using System;

namespace Media.Api.v1.Area.Media.ResultModels
{
    public class MediaFileResultModel
    {
        public Guid Id { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}
