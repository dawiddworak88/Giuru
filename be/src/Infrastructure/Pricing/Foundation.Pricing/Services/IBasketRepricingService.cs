using Foundation.Pricing.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public interface IBasketRepricingService
    {
        /// <summary>
        /// Prices every line whose SKU resolves in <paramref name="productLookup"/> and applies the
        /// result back onto <paramref name="lines"/>. SKU resolution, the "unresolved SKU" warnings and
        /// any product-identity checks are the caller's responsibility - this owns only the shared
        /// spine: build a <see cref="PriceProduct"/> per resolved line, ask
        /// <see cref="IPriceService.GetPriceResultsForBasketAsync"/>, align the response back onto
        /// <paramref name="lines"/> by index, and apply it via <see cref="BasketPriceApplier"/>.
        /// </summary>
        Task<RepriceOutcome> RepriceAsync<TLine, TProduct>(
            IList<TLine> lines,
            Func<TLine, string> skuSelector,
            Func<TLine, bool> isOutletSelector,
            IReadOnlyDictionary<string, TProduct> productLookup,
            Func<TProduct, bool, Task<PriceProduct>> priceProductFactory,
            PriceClient priceClient,
            DateTime pricingDate) where TLine : IPriceableBasketLine;
    }
}
