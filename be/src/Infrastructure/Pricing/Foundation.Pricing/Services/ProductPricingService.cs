using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public sealed class ProductPricingService : IProductPricingService
    {
        private readonly IPriceService _priceService;
        private readonly IPricingSettings _settings;
        private readonly ILogger<ProductPricingService> _logger;

        public ProductPricingService(IPriceService priceService, IPricingSettings settings, ILogger<ProductPricingService> logger)
        {
            _priceService = priceService;
            _settings = settings;
            _logger = logger;
        }

        public async Task<PricedProducts> GetPricesAsync(
            Func<Task<IEnumerable<PriceProduct>>> priceProductsFactory,
            Func<Task<PriceClient>> priceClientFactory,
            DateTime? pricingDate = null)
        {
            if (!_settings.IsGrulaConfigured)
            {
                return PricedProducts.Empty;
            }

            var priceClient = await priceClientFactory();

            if (!_priceService.CanSeePrices(priceClient?.Id))
            {
                return PricedProducts.Empty;
            }

            var priceProducts = (await priceProductsFactory())?.ToList();

            if (priceProducts is null || priceProducts.Count is 0)
            {
                return PricedProducts.Empty;
            }

            var prices = (await _priceService.GetPrices(
                pricingDate ?? DateTime.UtcNow, priceProducts, priceClient))?.ToList();

            if (prices is not null && prices.Count != priceProducts.Count)
            {
                // Prices are consumed positionally by the caller, so a short response would shift
                // every subsequent product's price onto the wrong product.
                _logger.LogWarning(
                    "Grula returned {PriceCount} prices for {ProductCount} catalog products; dropping the response.",
                    prices.Count, priceProducts.Count);

                return PricedProducts.Empty;
            }

            return new PricedProducts(prices);
        }

        public async Task<Price> GetPriceAsync(
            Func<Task<PriceProduct>> priceProductFactory,
            Func<Task<PriceClient>> priceClientFactory,
            DateTime? pricingDate = null)
        {
            if (!_settings.IsGrulaConfigured)
            {
                return null;
            }

            var priceClient = await priceClientFactory();

            if (!_priceService.CanSeePrices(priceClient?.Id))
            {
                return null;
            }

            var priceProduct = await priceProductFactory();

            if (priceProduct is null)
            {
                return null;
            }

            return await _priceService.GetPrice(pricingDate ?? DateTime.UtcNow, priceProduct, priceClient);
        }
    }
}
