using Foundation.Media.Configurations;
using Microsoft.Extensions.Options;
using System.Text;

namespace Foundation.Media.Services.CdnServices
{
    public class CloudinaryCdnService : ICdnService
    {
        private readonly IOptions<MediaAppSettings> options;

        public CloudinaryCdnService(
            IOptions<MediaAppSettings> options)
        {
            this.options = options;
        }

        public string GetCdnMediaUrl(string mediaUrl, int? maxWidth)
        {
            if (string.IsNullOrWhiteSpace(this.options.Value.CdnUrl))
            {
                return mediaUrl;
            }

            var sb = new StringBuilder(this.options.Value.CdnUrl);

            if (maxWidth.HasValue)
            {
                sb.Append($"/c_scale,w_{maxWidth}");
            }

            sb.Append("/q_auto/f_auto");

            sb.Append($"/{mediaUrl}");

            return sb.ToString();
        }
    }
}
