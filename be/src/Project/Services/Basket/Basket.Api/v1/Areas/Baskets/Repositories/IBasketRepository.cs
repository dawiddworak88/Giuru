using Basket.Api.v1.Areas.Baskets.Models;
using System;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Repositories
{
    public interface IBasketRepository
    {
        Task<BasketModel> GetBasketAsync(Guid id);
        Task<BasketModel> UpdateBasketAsync(BasketModel basket);
        Task<bool> DeleteBasketAsync(Guid? basketId);
    }
}
