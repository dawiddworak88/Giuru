using Buyer.Web.Shared.DomainModels.Baskets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Baskets
{
    public interface IBasketService
    {
        Task<IEnumerable<BasketItem>> GetBasketAsync(Guid? basketId, string token, string language);
        Task ValidateStockOutletQuantitiesAsync(Guid? basketId, string token, string language);
    }
}
