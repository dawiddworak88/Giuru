using System;

namespace Media.Api.v1.Area.Media.ResultModels
{
    public class MediaItemResultModel
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Size { get; set; }
        public bool IsProtected { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
