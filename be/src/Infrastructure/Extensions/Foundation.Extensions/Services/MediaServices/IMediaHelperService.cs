using System;

namespace Foundation.Extensions.Services.MediaServices
{
    public interface IMediaHelperService
    {
        string GetFileUrl(string baseUrl, Guid mediaId, string extension);
        string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight, string extension);
        string GetFileUrl(string baseUrl, int maxWidth, int maxHeight, string extension);
        string GetFileUrl(string baseUrl, int maxWidth, int maxHeight);
        public string GetFileUrl(string baseUrl, Guid mediaId);
        string GetFileUrl(string baseUrl, Guid mediaId, bool optimizeImage);
        public string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight);
        string GetFileUrl(string baseUrl, Guid mediaId, int maxWidth, int maxHeight, bool optimizeImage);
    }
}
