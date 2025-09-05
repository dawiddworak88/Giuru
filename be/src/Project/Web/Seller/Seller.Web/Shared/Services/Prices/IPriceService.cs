using Seller.Web.Shared.DomainModels.Prices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Prices
{
    public interface IPriceService
    {
        Task<IEnumerable<Price>> GetPrices(
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client);

        Task<Price> GetPrice(
            DateTime pricingDate,
            PriceProduct product,
            PriceClient client);
    }
}
