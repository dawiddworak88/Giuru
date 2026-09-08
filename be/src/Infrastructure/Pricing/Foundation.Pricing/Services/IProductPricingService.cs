using Foundation.Pricing.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The catalog counterpart to <see cref="IBasketRepricingService"/>. Owns the shared spine:
    /// the IsGrulaConfigured gate, lazy resolution of the client and the price products, the
    /// IPriceService call, and index-safe access to the positional response. Callers keep only
    /// what genuinely differs: where the client comes from and how a PriceProduct is built.
    /// </summary>
    public interface IProductPricingService
    {
        /// <summary>
        /// Both factories are lazy so the gate can short-circuit before either the client or the
        /// price products are built - in particular before a resolver's API calls run.
        /// </summary>
        Task<PricedProducts> GetPricesAsync(
            Func<Task<IEnumerable<PriceProduct>>> priceProductsFactory,
            Func<Task<PriceClient>> priceClientFactory,
            DateTime? pricingDate = null);

        Task<Price> GetPriceAsync(
            Func<Task<PriceProduct>> priceProductFactory,
            Func<Task<PriceClient>> priceClientFactory,
            DateTime? pricingDate = null);
    }
}
