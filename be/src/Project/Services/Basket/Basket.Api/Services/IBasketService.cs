using Basket.Api.ServicesModels;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public interface IBasketService
    {
        Task<BasketServiceModel> UpdateAsync(UpdateBasketServiceModel serviceModel);
        Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel);
    }
}
