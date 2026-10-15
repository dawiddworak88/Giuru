using System;
using System.Net.Http.Headers;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DiscountCodes;
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
            services.AddScoped<IProductPricingService, ProductPricingService>();
            services.AddScoped<IDiscountCodeLookup, ClientApiDiscountCodeLookup>();
            services.AddScoped<IDiscountCodeValidator, DiscountCodeValidator>();

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

        /// <summary>
        /// Registers the price client resolvers of an app from its one concrete resolver. Fresh product price
        /// reads get <see cref="IPriceClientResolver"/>, which validates a supplied discount code; basket saves
        /// get <see cref="IBasketPriceClientResolver"/>, which keeps the code the basket coordinator resolved.
        /// The decorator wraps the concrete resolver, never the decorated interface.
        /// </summary>
        public static void RegisterPriceClientResolvers<TResolver>(this IServiceCollection services)
            where TResolver : class, IBasketPriceClientResolver
        {
            services.AddScoped<TResolver>();
            services.AddScoped<IBasketPriceClientResolver>(sp => sp.GetRequiredService<TResolver>());
            services.AddScoped<IPriceClientResolver>(sp => new VerifiedDiscountCodePriceClientResolver(
                sp.GetRequiredService<TResolver>(),
                sp.GetRequiredService<IDiscountCodeValidator>(),
                sp.GetRequiredService<IPricingSettings>()));
        }
    }
}
