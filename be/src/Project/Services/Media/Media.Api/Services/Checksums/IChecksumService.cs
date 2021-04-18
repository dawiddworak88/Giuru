using Microsoft.AspNetCore.Http;
using System.IO;

namespace Media.Api.Services.Checksums
{
    public interface IChecksumService
    {
        string GetMd5(IFormFile file);
        string GetMd5(Stream stream);
    }
}
