using System;
using System.Net.Http.Headers;
using Foundation.Pricing.Services;
using Grula.PricingIntelligencePlatform.Sdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Pricing.DependencyInjection
{
    public static class PricingCompositionRoot
    {
        public static void RegisterPricingDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IBasketRepricingService, BasketRepricingService>();

            services.AddHttpClient("GrulaApi")
                .AddTypedClient(httpClient =>
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    var token = configuration["GrulaAccessToken"];
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    return new GrulaApiClient(configuration["GrulaUrl"], httpClient);
                });
        }
    }
}
