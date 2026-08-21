using Foundation.Pricing.DomainModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public sealed class BasketRepricingService : IBasketRepricingService
    {
        private readonly IPriceService _priceService;
        private readonly ILogger<BasketRepricingService> _logger;

        public BasketRepricingService(IPriceService priceService, ILogger<BasketRepricingService> logger)
        {
            _priceService = priceService;
            _logger = logger;
        }

        public async Task<RepriceOutcome> RepriceAsync<TLine, TProduct>(
            IList<TLine> lines,
            Func<TLine, string> skuSelector,
            Func<TLine, bool> isOutletSelector,
            IReadOnlyDictionary<string, TProduct> productLookup,
            Func<TProduct, bool, Task<PriceProduct>> priceProductFactory,
            PriceClient priceClient,
            DateTime pricingDate) where TLine : IPriceableBasketLine
        {
            if (lines is null || lines.Count is 0)
            {
                return new RepriceOutcome(true);
            }

            var indexedLines = lines
                .Select((line, index) => (Line: line, Sku: skuSelector(line), Index: index))
                .Where(x => !string.IsNullOrWhiteSpace(x.Sku) && productLookup.ContainsKey(x.Sku))
                .ToList();

            var priceProducts = await Task.WhenAll(indexedLines.Select(x =>
                priceProductFactory(productLookup[x.Sku], isOutletSelector(x.Line))));

            var prices = await _priceService.GetPriceResultsForBasketAsync(pricingDate, priceProducts, priceClient);

            var alignedPrices = BasketPriceApplier.AlignPrices(
                lines.Count,
                indexedLines.Select(x => x.Index).ToList(),
                prices?.ToList());

            if (alignedPrices is null)
            {
                _logger.LogWarning(
                    "Grula returned {PriceResultCount} price results for {PricedLineCount} priced basket lines; the basket will be persisted unpriced.",
                    prices?.Count,
                    indexedLines.Count);
            }

            var succeeded = BasketPriceApplier.ApplyPrices(lines, alignedPrices);

            return new RepriceOutcome(succeeded);
        }
    }
}
