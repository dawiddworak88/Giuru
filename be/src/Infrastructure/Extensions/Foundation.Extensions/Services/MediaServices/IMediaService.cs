using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public interface IMediaService
    {
        public string GetMediaUrl(string baseUrl, Guid mediaId);
        public string GetMediaUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight);
    }
}
