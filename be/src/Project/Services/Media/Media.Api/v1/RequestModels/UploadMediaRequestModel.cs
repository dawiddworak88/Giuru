using Microsoft.AspNetCore.Http;

namespace Media.Api.v1.Areas.Media.RequestModels
{
    public class UploadMediaRequestModel
    {
        public string Id { get; set; }
        public int? ChunkNumber { get; set; }
        public IFormFile File { get; set; }
    }
}
