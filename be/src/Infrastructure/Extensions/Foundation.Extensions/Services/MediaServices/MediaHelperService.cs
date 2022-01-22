using Foundation.Extensions.Definitions;
using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public class MediaHelperService : IMediaHelperService
    {
        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight)
        {
            var url = this.GetFileUrl(baseUrl, mediaId);

            url += $"?w={maxWidth}&h={maxHeight}";

            return url;
        }

        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight, bool optimizeImage)
        {
            var url = this.GetFileUrl(baseUrl, mediaId, maxWidth, maxHeight);

            if (optimizeImage)
            {
                url += $"&o=true";
            }

            return url;
        }

        public string GetFileUrl(string baseUrl, Guid mediaId, bool optimizeImage)
        {
            var url = this.GetFileUrl(baseUrl, mediaId);

            if (optimizeImage)
            {
                url += $"?o=true";
            }

            return url;
        }

        public string GetFileUrl(string baseUrl, Guid mediaId)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || mediaId == Guid.Empty)
            {
                return string.Empty;
            }

            return $"{baseUrl}{EndpointConstants.Files.FilesApiEndpoint}/{mediaId}";
        }

        public string GetFileUrl(string baseUrl, Guid mediaId, string extension)
        {
            var url = this.GetFileUrl(baseUrl, mediaId);

            if (!string.IsNullOrWhiteSpace(extension))
            {
                url += $"?extension={extension}";
            }

            return url;
        }

        public string GetFileUrl(string baseUrl, int maxWidth, int maxHeight, string extension)
        {
            var url = this.GetFileUrl(baseUrl, maxWidth, maxHeight);

            if (string.IsNullOrWhiteSpace(extension) is false)
            {
                url += $"&extension={extension}";
            }

            return url;
        }

        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight, string extension)
        {
            var url = this.GetFileUrl(baseUrl, mediaId, maxWidth, maxHeight);

            if (string.IsNullOrWhiteSpace(extension) is false)
            {
                url += $"&extension={extension}";
            }

            return url;
        }

        public string GetFileUrl(string baseUrl, int maxWidth, int maxHeight)
        {
            return $"{baseUrl}?w={maxWidth}&h={maxHeight}";
        }
    }
}
