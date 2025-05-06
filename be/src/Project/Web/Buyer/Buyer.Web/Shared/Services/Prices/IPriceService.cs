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
            DateTime pricingDate,
            PriceProduct product,
            PriceClient client);

        Task<IEnumerable<Price>> GetPrices(
            string token,
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client);
    }
}
