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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiBasketService = Basket.Api.Services.BasketService;

namespace Giuru.UnitTests.Services.Baskets
{
    // The snapshot guard of Basket.Api: a web caller validates a discount code against the basket it read, so checkout has
    // to order exactly that snapshot or refuse. Every event is built from the one read made by CheckoutAsync.
    public class BasketSnapshotGuardTests
    {
        private static readonly Guid BasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid ProductId = Guid.NewGuid();

        private static ApiBasketService CreateService(IBasketRepository basketRepository, IEventBus eventBus)
        {
            var orderLocalizer = Substitute.For<IStringLocalizer<OrderResources>>();
            orderLocalizer["BasketNotFound"].Returns(new LocalizedString("BasketNotFound", "Basket not found."));
            orderLocalizer["DiscountCodeBasketNeedsRefresh"].Returns(new LocalizedString("DiscountCodeBasketNeedsRefresh", "Save the basket."));

            return new ApiBasketService(basketRepository, eventBus, orderLocalizer);
        }

        private static BasketRepositoryModel CreateBasket(Guid? version, string discountCode = "SUMMER25", int quantity = 2)
        {
            return new BasketRepositoryModel
            {
                Id = BasketId,
                DiscountCode = discountCode,
                DiscountCodeClientId = discountCode is null ? null : ClientId,
                BasketVersion = version,
                Items = new[]
                {
                    new BasketItemRepositoryModel { ProductId = ProductId, ProductSku = "SKU", ProductName = "Product", Quantity = quantity, StockQuantity = quantity, OutletQuantity = 0 }
                }
            };
        }

        private static IBasketRepository RepositoryHolding(BasketRepositoryModel basket)
        {
            var repository = Substitute.For<IBasketRepository>();
            repository.GetBasketAsync(BasketId).Returns(Task.FromResult(basket));

            return repository;
        }

        private static CheckoutBasketServiceModel Checkout(Guid? expectedVersion, bool hasCustomOrder = false)
        {
            return new CheckoutBasketServiceModel
            {
                BasketId = BasketId,
                ClientId = ClientId,
                ExpectedBasketVersion = expectedVersion,
                HasCustomOrder = hasCustomOrder
            };
        }

        private static void AssertNothingPublished(IEventBus eventBus)
        {
            eventBus.DidNotReceiveWithAnyArgs().Publish(default(IntegrationEvent));
        }

        private static T PublishedEvent<T>(IEventBus eventBus) where T : IntegrationEvent
        {
            return eventBus.ReceivedCalls()
                .Select(x => x.GetArguments().FirstOrDefault())
                .OfType<T>()
                .Single();
        }

        [Fact]
        public async Task CheckoutAsync_WhenTheExpectedVersionDiffersFromTheStoredOne_ThrowsConflictBeforeAnyEvent()
        {
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(Guid.NewGuid())), eventBus);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(Guid.NewGuid())));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WhenTheStoredBasketHasNoVersion_ThrowsConflictBeforeAnyEvent()
        {
            // A basket saved before versions existed has to be saved once before a guarded checkout.
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(null)), eventBus);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(Guid.NewGuid())));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WhenTheBasketIsGoneAndAVersionIsExpected_ThrowsConflict()
        {
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(null), eventBus);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(Guid.NewGuid(), hasCustomOrder: true)));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WhenTheVersionMatches_BuildsTheEventsFromTheCheckedSnapshot()
        {
            var version = Guid.NewGuid();
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(version, quantity: 3)), eventBus);

            await service.CheckoutAsync(Checkout(version));

            var accepted = PublishedEvent<BasketCheckoutAcceptedIntegrationEvent>(eventBus);
            var stock = PublishedEvent<BasketCheckoutStockProductsIntegrationEvent>(eventBus);

            Assert.Equal("SUMMER25", accepted.DiscountCode);
            Assert.Equal(3, Assert.Single(accepted.Basket.Items).Quantity);
            Assert.Equal(3, Assert.Single(stock.Items).BookedQuantity);
        }

        [Fact]
        public async Task CheckoutAsync_ReadsTheBasketOnce_SoALaterSaveCannotChangeTheEvents()
        {
            var version = Guid.NewGuid();
            var repository = Substitute.For<IBasketRepository>();

            // The first read is the validated snapshot. A save that lands afterwards would be visible to a second read only.
            repository.GetBasketAsync(BasketId).Returns(
                Task.FromResult(CreateBasket(version, "SUMMER25")),
                Task.FromResult(CreateBasket(Guid.NewGuid(), "OTHER")));

            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(repository, eventBus);

            await service.CheckoutAsync(Checkout(version));

            await repository.Received(1).GetBasketAsync(BasketId);
            Assert.Equal("SUMMER25", PublishedEvent<BasketCheckoutAcceptedIntegrationEvent>(eventBus).DiscountCode);
        }

        [Fact]
        public async Task CheckoutAsync_WhenACodeWasIntroducedToACodeFreeBasketAfterTheCallerRead_ThrowsConflict()
        {
            // The caller validated a basket without a code. A concurrent save then added one and issued a new version.
            var versionTheCallerSaw = Guid.NewGuid();
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(Guid.NewGuid(), "SUMMER25")), eventBus);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(versionTheCallerSaw)));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WhenAbsenceIsAssertedAndNoBasketExists_Proceeds()
        {
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(null), eventBus);

            await service.CheckoutAsync(Checkout(Guid.Empty, hasCustomOrder: true));

            eventBus.Received(1).Publish(Arg.Any<BasketCheckoutAcceptedIntegrationEvent>());
        }

        [Fact]
        public async Task CheckoutAsync_WhenAbsenceIsAssertedAndABasketWithLinesNowExists_ThrowsConflict()
        {
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(Guid.NewGuid(), null)), eventBus);

            var exception = await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(Guid.Empty, hasCustomOrder: true)));

            Assert.Equal((int)HttpStatusCode.Conflict, exception.StatusCode);
            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WhenAbsenceIsAssertedAndAnEmptyBasketWithoutACodeExists_Proceeds()
        {
            var empty = new BasketRepositoryModel { Id = BasketId, Items = Array.Empty<BasketItemRepositoryModel>() };
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(empty), eventBus);

            await service.CheckoutAsync(Checkout(Guid.Empty, hasCustomOrder: true));

            eventBus.Received(1).Publish(Arg.Any<BasketCheckoutAcceptedIntegrationEvent>());
        }

        [Fact]
        public async Task CheckoutAsync_WhenAbsenceIsAssertedAndAnEmptyBasketHoldsACode_ThrowsConflict()
        {
            var emptyWithCode = new BasketRepositoryModel
            {
                Id = BasketId,
                DiscountCode = "SUMMER25",
                Items = Array.Empty<BasketItemRepositoryModel>()
            };
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(emptyWithCode), eventBus);

            await Assert.ThrowsAsync<CustomException>(() => service.CheckoutAsync(Checkout(Guid.Empty, hasCustomOrder: true)));

            AssertNothingPublished(eventBus);
        }

        [Fact]
        public async Task CheckoutAsync_WithoutAnExpectedVersion_KeepsTheLegacyContract()
        {
            var eventBus = Substitute.For<IEventBus>();
            var service = CreateService(RepositoryHolding(CreateBasket(null)), eventBus);

            await service.CheckoutAsync(Checkout(null));

            eventBus.Received(1).Publish(Arg.Any<BasketCheckoutAcceptedIntegrationEvent>());
        }

        [Fact]
        public async Task UpdateAsync_GeneratesANewVersionForEveryWrite()
        {
            var repository = Substitute.For<IBasketRepository>();
            repository.UpdateBasketAsync(Arg.Any<BasketRepositoryModel>()).Returns(call => Task.FromResult(call.Arg<BasketRepositoryModel>()));
            var service = CreateService(repository, Substitute.For<IEventBus>());

            var first = await service.UpdateAsync(new UpdateBasketServiceModel { Id = BasketId, DiscountCode = "SUMMER25", DiscountCodeClientId = ClientId });
            var second = await service.UpdateAsync(new UpdateBasketServiceModel { Id = BasketId, DiscountCode = "SUMMER25", DiscountCodeClientId = ClientId });

            Assert.NotNull(first.BasketVersion);
            Assert.NotEqual(Guid.Empty, first.BasketVersion);
            Assert.NotEqual(first.BasketVersion, second.BasketVersion);
        }

        [Fact]
        public async Task UpdateAsync_StoresTheClientOnlyWithACode()
        {
            var repository = Substitute.For<IBasketRepository>();
            repository.UpdateBasketAsync(Arg.Any<BasketRepositoryModel>()).Returns(call => Task.FromResult(call.Arg<BasketRepositoryModel>()));
            var service = CreateService(repository, Substitute.For<IEventBus>());

            var withCode = await service.UpdateAsync(new UpdateBasketServiceModel { Id = BasketId, DiscountCode = "SUMMER25", DiscountCodeClientId = ClientId });
            var cleared = await service.UpdateAsync(new UpdateBasketServiceModel { Id = BasketId, DiscountCode = null, DiscountCodeClientId = ClientId });

            Assert.Equal(ClientId, withCode.DiscountCodeClientId);
            Assert.Null(cleared.DiscountCodeClientId);
        }

        [Fact]
        public async Task GetBasketById_CarriesTheStoredClientAndVersion()
        {
            var version = Guid.NewGuid();
            var service = CreateService(RepositoryHolding(CreateBasket(version)), Substitute.For<IEventBus>());

            var basket = await service.GetBasketById(new GetBasketByIdServiceModel { Id = BasketId });

            Assert.Equal(version, basket.BasketVersion);
            Assert.Equal(ClientId, basket.DiscountCodeClientId);
        }

        [Fact]
        public async Task GetBasketById_ForABasketThatDoesNotExist_HasNoVersion()
        {
            var service = CreateService(RepositoryHolding(null), Substitute.For<IEventBus>());

            var basket = await service.GetBasketById(new GetBasketByIdServiceModel { Id = BasketId });

            Assert.Null(basket.BasketVersion);
            Assert.Null(basket.DiscountCodeClientId);
        }
    }
}
