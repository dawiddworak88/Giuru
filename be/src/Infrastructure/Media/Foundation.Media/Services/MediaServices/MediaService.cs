using Foundation.Media.Configurations;
using Foundation.Media.Services.CdnServices;
using Microsoft.Extensions.Options;
using System;

namespace Foundation.Media.Services.MediaServices
{
    public class MediaService : IMediaService
    {
        private readonly IOptions<MediaAppSettings> options;
        private readonly ICdnService cdnService;

        public MediaService(
            IOptions<MediaAppSettings> options,
            ICdnService cdnService)
        {
            this.options = options;
            this.cdnService = cdnService;
        }

        public string ConvertToMB(long size)
        {
            return string.Format("{0:0.00} MB", size / 1024f / 1024f);
        }

        public string GetMediaUrl(Guid mediaId, int? maxWidth)
        {
            var mediaUrl = $"{this.options.Value.MediaUrl}/api/v1/files/{mediaId}";
            return this.cdnService.GetCdnMediaUrl(mediaUrl, maxWidth);
        }

        public string GetMediaUrl(string mediaUrl, int? maxWidth = null)
        {
            return this.cdnService.GetCdnMediaUrl(mediaUrl, maxWidth);
        }

        public string GetMediaVersionUrl(Guid mediaVersionId, int? maxWidth = null)
        {
            var mediaUrl = $"{this.options.Value.MediaUrl}/api/v1/files/version/{mediaVersionId}";
            return this.cdnService.GetCdnMediaUrl(mediaUrl, maxWidth);
        }

        public string GetNonCdnMediaUrl(Guid mediaId, int? maxWidth = null)
        {
            return $"{this.options.Value.MediaUrl}/api/v1/files/{mediaId}";
        }
    }
}
