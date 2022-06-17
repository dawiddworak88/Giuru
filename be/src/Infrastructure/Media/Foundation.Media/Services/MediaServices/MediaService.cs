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

        public string GetMediaUrl(Guid mediaId, int? maxWidth)
        {
            var mediaUrl = $"{this.options.Value.MediaUrl}/api/v1/files/{mediaId}";

            if (string.IsNullOrWhiteSpace(this.options.Value.CdnUrl) && maxWidth.HasValue)
            {
                mediaUrl += $"?o=true&w={maxWidth}&h={maxWidth}";
            }

            return this.cdnService.GetCdnMediaUrl(mediaUrl, maxWidth);
        }

        public string GetMediaUrl(string mediaUrl, int? maxWidth = null)
        {
            return this.cdnService.GetCdnMediaUrl(mediaUrl, maxWidth);
        }

        public string GetNonCdnMediaUrl(Guid mediaId, int? maxWidth = null)
        {
            var mediaUrl = $"{this.options.Value.MediaUrl}/api/v1/files/{mediaId}";

            if (string.IsNullOrWhiteSpace(this.options.Value.CdnUrl) && maxWidth.HasValue)
            {
                mediaUrl += $"?o=true&w={maxWidth}&h={maxWidth}";
            }

            return mediaUrl;
        }
    }
}
