using Basket.Api.v1.Areas.Baskets.Models;
using Basket.Api.v1.Areas.Baskets.Repositories;
using Basket.Api.v1.Areas.Baskets.ResultModels;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.v1.Areas.Baskets.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<BasketResultModel> UpdateAsync(UpdateBasketServiceModel serviceModel)
        {
            var basketModel = new BasketModel
            { 
                Id = serviceModel.Id,
                Items = serviceModel.Items.OrEmptyIfNull().Select(x => new BasketItemServiceModel 
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var result = await this.basketRepository.UpdateBasketAsync(basketModel);

            return new BasketResultModel
            { 
                Id = result.Id,
                Items = result.Items.OrEmptyIfNull().Select(x => new BasketItemResultModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };
        }
    }
}
