using Buyer.Web.Shared.DomainModels.Prices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    public interface IPriceService
    {
        Task<Price> GetPrice(
            string token,
            string currencyCode,
            DateTime pricingDate,
            PriceProduct product);

        Task<IEnumerable<Price>> GetPrices(
            string token,
            string currencyCode,
            DateTime pricingDate,
            IEnumerable<PriceProduct> products);
    }
}
