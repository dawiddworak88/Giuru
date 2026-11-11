using Foundation.Pricing.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public interface IPriceService
    {
        Task<Price> GetPrice(
            DateTime pricingDate,
            PriceProduct product,
            PriceClient client);

        Task<IEnumerable<Price>> GetPrices(
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client);

        /// <summary>Returns one explicit pricing outcome per supplied product for trusted basket persistence.</summary>
        Task<IReadOnlyList<PriceLookupResult>> GetPriceResultsForBasketAsync(
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client);

        bool CanSeePrices(Guid? priceClientId);
    }
}
