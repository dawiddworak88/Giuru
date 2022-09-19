using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Media.Api.ServicesModels
{
    public class CreateFileChunkServiceModel : BaseServiceModel
    {
        public IFormFile File { get; set; }
        public int? ChunkSumber { get; set; }
    }
}
