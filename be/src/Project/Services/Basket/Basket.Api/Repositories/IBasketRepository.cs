using Basket.Api.RepositoriesModels;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<BasketRepositoryModel> GetBasketAsync(Guid id);
        Task<BasketRepositoryModel> UpdateBasketAsync(BasketRepositoryModel basket);
        Task<bool> DeleteBasketAsync(Guid? basketId);
    }
}
