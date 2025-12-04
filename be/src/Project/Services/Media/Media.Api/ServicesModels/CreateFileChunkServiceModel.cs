using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Media.Api.ServicesModels
{
    public class CreateFileChunkServiceModel : BaseServiceModel
    {
        public string UploadId { get; set; }
        public IFormFile File { get; set; }
        public int? ChunkSumber { get; set; }
    }
}
