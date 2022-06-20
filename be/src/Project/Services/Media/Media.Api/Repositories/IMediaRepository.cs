using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Media.Api.Repositories
{
    public interface IMediaRepository
    {
        byte[] GetFile(string folder, string filename);
        Task CreateFileAsync(Guid mediaItemVersionId, string folderName, IFormFile file, string filename);
    }
}
