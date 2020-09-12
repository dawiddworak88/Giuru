using Azure.Storage.Blobs;
using Media.Api.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IOptions<AppSettings> configuration;

        public MediaRepository(
            IOptions<AppSettings> configuration)
        {
            this.configuration = configuration;
        }

        public async Task<byte[]> GetFileAsync(string folder, string filename)
        {
            var blobServiceClient = new BlobServiceClient(this.configuration.Value.StorageConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(folder);

            if (containerClient != null)
            {
                using (var ms = new MemoryStream())
                {
                    await containerClient.GetBlobClient(filename).DownloadToAsync(ms);

                    return ms.ToArray();
                }
            }

            return default;
        }

        public async Task CreateFileAsync(Guid mediaItemVersionId, string folderName, IFormFile file)
        {
            var container = new BlobContainerClient(this.configuration.Value.StorageConnectionString, folderName);

            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient($"{mediaItemVersionId}{Path.GetExtension(file.FileName)}");

            if (!blob.Exists())
            {
                blob.Upload(file.OpenReadStream());
            }
        }
    }
}
