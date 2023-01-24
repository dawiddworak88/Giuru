using Azure.Storage.Blobs;
using Media.Api.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Media.Api.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IOptions<AppSettings> _configuration;

        public MediaRepository(
            IOptions<AppSettings> configuration)
        {
            _configuration = configuration;
        }

        public byte[] GetFile(string folder, string filename)
        {
            var blobServiceClient = new BlobServiceClient(_configuration.Value.StorageConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(folder);

            if (containerClient != null)
            {
                using (var ms = new MemoryStream())
                {
                    containerClient.GetBlobClient(filename).DownloadTo(ms);

                    return ms.ToArray();
                }
            }

            return default;
        }

        public async Task CreateFileAsync(Guid mediaItemVersionId, string folderName, IFormFile file, string filename)
        {
            var container = new BlobContainerClient(_configuration.Value.StorageConnectionString, folderName);

            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient($"{mediaItemVersionId}{Path.GetExtension(filename)}");

            if (!blob.Exists())
            {
                using (var stream = file.OpenReadStream())
                {
                    blob.Upload(stream);
                }
            }
        }
    }
}
