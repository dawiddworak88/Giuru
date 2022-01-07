using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public interface IMediaHelperService
    {
        public string GetFileUrl(string baseUrl, Guid mediaId);
        string GetFileUrl(string baseUrl, Guid mediaId, bool optimizeImage);
        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight);
        string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight, bool optimizeImage);
    }
}
