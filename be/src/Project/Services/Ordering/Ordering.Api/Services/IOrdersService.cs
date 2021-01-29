using Ordering.Api.ServicesModels;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public interface IOrdersService
    {
        Task CheckoutAsync(CheckoutBasketServiceModel serviceModel);
    }
}
