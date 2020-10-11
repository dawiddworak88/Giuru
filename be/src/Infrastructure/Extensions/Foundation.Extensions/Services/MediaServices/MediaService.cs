using Foundation.Extensions.Definitions;
using Microsoft.AspNetCore.Http;
using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public class MediaService : IMediaService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MediaService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetMediaUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight)
        {
            var url = this.GetMediaUrl(baseUrl, mediaId);

            url += $"?w={maxWidth}&h={maxHeight}";

            return url;
        }

        public string GetMediaUrl(string baseUrl, Guid mediaId)
        {
            return $"{this.httpContextAccessor.HttpContext.Request.Scheme}://{baseUrl}{EndpointConstants.Media.MediaApiEndpoint}/{mediaId}";
        }
    }
}
