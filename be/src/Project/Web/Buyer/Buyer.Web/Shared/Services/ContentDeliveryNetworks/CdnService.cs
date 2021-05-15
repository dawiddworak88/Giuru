using Buyer.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using System;

namespace Buyer.Web.Shared.Services.ContentDeliveryNetworks
{
    public class CdnService : ICdnService
    {
        private readonly IOptions<AppSettings> options;

        public CdnService(IOptions<AppSettings> options)
        {
            this.options = options;
        }

        public string GetCdnUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(this.options.Value.CdnUrl) && !string.IsNullOrWhiteSpace(url) && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
            {
                return $"{this.options.Value.CdnUrl}{uri.AbsolutePath}{uri.Query}";
            }

            return url;
        }
    }
}
