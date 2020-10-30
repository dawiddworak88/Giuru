using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public interface IMediaHelperService
    {
        public string GetFileUrl(string baseUrl, Guid mediaId);
        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight);
    }
}
