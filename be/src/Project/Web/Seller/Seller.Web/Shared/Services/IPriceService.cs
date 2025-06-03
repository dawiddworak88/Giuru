using Seller.Web.Shared.DomainModels.Prices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services
{
    public interface IPriceService
    {
        Task<IEnumerable<Price>> GetPrices(
            string token,
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client);
    }
}
