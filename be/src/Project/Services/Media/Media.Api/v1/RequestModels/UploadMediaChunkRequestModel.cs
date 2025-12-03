using Microsoft.AspNetCore.Http;
using System;

namespace Media.Api.v1.RequestModels
{
    public class UploadMediaChunkRequestModel
    {
        public Guid UploadId { get; set; }
        public int? ChunkNumber { get; set; }
        public IFormFile File { get; set; }
    }
}
