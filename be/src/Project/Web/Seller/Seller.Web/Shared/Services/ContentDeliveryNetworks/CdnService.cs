using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using System;

namespace Seller.Web.Shared.Services.ContentDeliveryNetworks
{
    public class CdnService : ICdnService
    {
        private readonly IOptions<AppSettings> options;
        private readonly ILogger logger;

        public CdnService(
            IOptions<AppSettings> options,
            ILogger<CdnService> logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public string GetCdnUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(this.options.Value.CdnUrl) && !string.IsNullOrWhiteSpace(url))
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                {
                    return $"{this.options.Value.CdnUrl}{uri.AbsolutePath}{uri.Query}";
                }
                else
                {
                    this.logger.LogError("Couldn't create CDN Uri for " + url);
                }
            }

            return url;
        }
    }
}
