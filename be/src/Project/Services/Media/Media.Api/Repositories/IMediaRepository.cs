using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Media.Api.Repositories
{
    public interface IMediaRepository
    {
        Task<byte[]> GetFileAsync(string folder, string filename);
        Task CreateFileAsync(Guid mediaItemVersionId, string folderName, IFormFile file, string filename);
    }
}
