using Microsoft.AspNetCore.Http;
using System.IO;

namespace Media.Api.Shared.Checksums
{
    public interface IChecksumService
    {
        string GetMd5(IFormFile file);
        string GetMd5(Stream stream);
    }
}
