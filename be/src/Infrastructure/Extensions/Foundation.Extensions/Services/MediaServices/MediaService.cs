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

        public string GetMediaUrl(string baseUrl, Guid mediaId)
        {
            return $"{this.httpContextAccessor.HttpContext.Request.Scheme}://{baseUrl}{ApiConstants.Media.MediaApiEndpoint}/{mediaId}";
        }
    }
}
