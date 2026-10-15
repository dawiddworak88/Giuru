using Basket.Api.Repositories;
using Basket.Api.RepositoriesModels;
using Basket.Api.ServicesModels;
using Foundation.EventBus.Abstractions;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;
using System.Threading.Tasks;
using ApiBasketService = Basket.Api.Services.BasketService;

namespace Giuru.UnitTests.Services.Baskets
{
    public class BasketApiUpdateTests
    {
        [Fact]
        public async Task UpdateAsync_StoresTheClientTheBasketWasPricedFor_AndTheBasketIsReadBackWithIt()
        {
            // The seller's checkout compares this client with the one the order is placed for.
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            BasketRepositoryModel stored = null;
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.UpdateBasketAsync(Arg.Any<BasketRepositoryModel>())
                .Returns(call => Task.FromResult(stored = call.Arg<BasketRepositoryModel>()));
            basketRepository.GetBasketAsync(basketId).Returns(_ => Task.FromResult(stored));

            var service = new ApiBasketService(basketRepository, Substitute.For<IEventBus>(), Substitute.For<IStringLocalizer<OrderResources>>());

            var saved = await service.UpdateAsync(new UpdateBasketServiceModel { Id = basketId, ClientId = clientId, DiscountCode = "SUMMER25" });
            var read = await service.GetBasketById(new GetBasketByIdServiceModel { Id = basketId });

            Assert.Equal(clientId, stored.ClientId);
            Assert.Equal(clientId, saved.ClientId);
            Assert.Equal(clientId, read.ClientId);
        }
    }
}
