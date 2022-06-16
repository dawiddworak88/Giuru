namespace Foundation.Media.Services.CdnServices
{
    public interface ICdnService
    {
        string GetCdnMediaUrl(string mediaUrl, int? maxWidth);
    }
}
