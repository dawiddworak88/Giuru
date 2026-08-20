using Basket.Api.IntegrationEvents;
using Basket.Api.Repositories;
using Basket.Api.RepositoriesModels;
using Basket.Api.ServicesModels;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus.Events;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using ApiBasketService = Basket.Api.Services.BasketService;

namespace Giuru.UnitTests.Services.Baskets
{
    public class BasketApiCheckoutTests
    {
        private static ApiBasketService CreateService(
            IBasketRepository basketRepository,
            IEventBus eventBus,
            out IStringLocalizer<OrderResources> orderLocalizer)
        {
            orderLocalizer = Substitute.For<IStringLocalizer<OrderResources>>();
            orderLocalizer.GetString("BasketNotFound").Returns(new LocalizedString("BasketNotFound", "Basket not found."));

            return new ApiBasketService(basketRepository, eventBus, orderLocalizer);
        }

        [Fact]
        public async Task CheckoutAsync_WhenBasketIsNullAndHasCustomOrder_PublishesCheckoutEventWithoutThrowing()
        {
            var basketId = Guid.NewGuid();
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketAsync(basketId).Returns(Task.FromResult((BasketRepositoryModel)null));
            var eventBus = Substitute.For<IEventBus>();

            var service = CreateService(basketRepository, eventBus, out _);

            await service.CheckoutAsync(new CheckoutBasketServiceModel
            {
                BasketId = basketId,
                HasCustomOrder = true,
                MoreInfo = "Custom order details"
            });

            eventBus.Received(1).Publish(Arg.Is<IntegrationEvent>(e => e is BasketCheckoutAcceptedIntegrationEvent));
        }

        [Fact]
        public async Task CheckoutAsync_WhenBasketIsNullAndHasCustomOrder_DoesNotPublishStockOrOutletBookingEvents()
        {
            var basketId = Guid.NewGuid();
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketAsync(basketId).Returns(Task.FromResult((BasketRepositoryModel)null));
            var eventBus = Substitute.For<IEventBus>();

            var service = CreateService(basketRepository, eventBus, out _);

            await service.CheckoutAsync(new CheckoutBasketServiceModel
            {
                BasketId = basketId,
                HasCustomOrder = true,
                MoreInfo = "Custom order details"
            });

            eventBus.DidNotReceive().Publish(Arg.Is<IntegrationEvent>(e => e is BasketCheckoutStockProductsIntegrationEvent));
            eventBus.DidNotReceive().Publish(Arg.Is<IntegrationEvent>(e => e is BasketCheckoutOutletProductsIntegrationEvent));
        }

        [Fact]
        public async Task CheckoutAsync_WhenBasketExistsWithNullItems_DoesNotThrow()
        {
            var basketId = Guid.NewGuid();
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketAsync(basketId).Returns(Task.FromResult(new BasketRepositoryModel
            {
                Id = basketId,
                Items = null
            }));
            var eventBus = Substitute.For<IEventBus>();

            var service = CreateService(basketRepository, eventBus, out _);

            await service.CheckoutAsync(new CheckoutBasketServiceModel
            {
                BasketId = basketId,
                HasCustomOrder = true,
                MoreInfo = "Custom order details"
            });

            eventBus.Received(1).Publish(Arg.Is<IntegrationEvent>(e => e is BasketCheckoutAcceptedIntegrationEvent));
        }

        [Fact]
        public async Task CheckoutAsync_WhenBasketIsNullAndHasCustomOrderIsFalse_ThrowsNotFound()
        {
            var basketId = Guid.NewGuid();
            var basketRepository = Substitute.For<IBasketRepository>();
            basketRepository.GetBasketAsync(basketId).Returns(Task.FromResult((BasketRepositoryModel)null));
            var eventBus = Substitute.For<IEventBus>();

            var service = CreateService(basketRepository, eventBus, out _);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(new CheckoutBasketServiceModel
            {
                BasketId = basketId,
                HasCustomOrder = false
            }));

            Assert.Equal((int)HttpStatusCode.NotFound, exception.StatusCode);
        }
    }
}
