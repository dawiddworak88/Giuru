using System;
using System.Collections.Generic;

namespace Media.Api.v1.ResponseModels
{
    public class MediaItemResponseModel
    {
        public Guid Id { get; set; }
        public Guid? MediaItemVersionId { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public bool IsProtected { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
