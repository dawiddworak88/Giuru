using Basket.Api.ServicesModels;
using System.Threading.Tasks;

namespace Basket.Api.Services
{
    public interface IBasketService
    {
        Task<BasketServiceModel> UpdateAsync(UpdateBasketServiceModel serviceModel);
        Task DeleteAsync(DeleteBasketServiceModel serviceModel);
        Task<BasketOrderServiceModel> DelteItemAsync(DeleteBasketItemServiceModel serviceModel);
        Task<BasketOrderServiceModel> GetByOrganisation(GetBasketByOrganisationServiceModel serviceModel);
        Task CheckoutAsync(CheckoutBasketServiceModel checkoutBasketServiceModel);
    }
}
