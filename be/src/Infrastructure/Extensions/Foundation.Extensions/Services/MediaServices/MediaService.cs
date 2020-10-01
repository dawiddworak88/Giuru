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

        public string GetMediaUrl(string baseUrl, Guid mediaId, int? maxWidth = null, int? maxHeight = null)
        {
            var url = $"{this.httpContextAccessor.HttpContext.Request.Scheme}://{baseUrl}{EndpointConstants.Media.MediaApiEndpoint}/{mediaId}";

            if (maxWidth.HasValue && maxHeight.HasValue)
            {
                url += $"?w={maxWidth.Value}&h={maxHeight.Value}";
            }

            return url;
        }
    }
}
