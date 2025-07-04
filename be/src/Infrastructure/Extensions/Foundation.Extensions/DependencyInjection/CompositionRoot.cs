using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using System.Net;

namespace Foundation.Extensions.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterGeneralDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear(); // loopback by default, clear it for K8s
                options.KnownProxies.Add(IPAddress.Parse("20.223.248.47"));
            });
        }
    }
}
