using Basket.Api.v1.Areas.Baskets.Models;
using Basket.Api.v1.Areas.Baskets.ResultModels;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Services
{
    public interface IBasketService
    {
        Task<BasketResultModel> UpdateAsync(UpdateBasketServiceModel serviceModel);
    }
}
