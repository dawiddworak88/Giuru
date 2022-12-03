using System;

namespace Foundation.Media.Services.MediaServices
{
    public interface IMediaService
    {
        string GetMediaUrl(Guid mediaId, int? maxWidth = null);
        string GetMediaUrl(string mediaUrl, int? maxWidth = null);
        string GetMediaVersionUrl(Guid mediaVersionId, int? maxWidth = null);
        string GetNonCdnMediaUrl(Guid mediaId, int? maxWidth = null);
        string ConvertToMB(long size);
    }
}
