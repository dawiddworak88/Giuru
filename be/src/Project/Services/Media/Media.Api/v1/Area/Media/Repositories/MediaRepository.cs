using Azure.Storage;
using Azure.Storage.Blobs;
using Media.Api.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IOptions<AppSettings> configuration;

        public MediaRepository(IOptions<AppSettings> configuration)
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
    }
}
