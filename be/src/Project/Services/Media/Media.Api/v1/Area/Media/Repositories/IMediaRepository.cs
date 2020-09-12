using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Repositories
{
    public interface IMediaRepository
    {
        Task<byte[]> GetFileAsync(string folder, string filename);
        Task CreateFileAsync(Guid mediaItemVersionId, string folderName, IFormFile file);
    }
}
