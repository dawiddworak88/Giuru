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

        public string GetFileUrl(string baseUrl, Guid mediaId)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || mediaId == Guid.Empty)
            {
                return string.Empty;
            }

            return $"{baseUrl}{EndpointConstants.Files.FilesApiEndpoint}/{mediaId}";
        }
    }
}
