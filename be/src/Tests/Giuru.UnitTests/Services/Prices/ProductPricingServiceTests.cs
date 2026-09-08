using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.Prices
{
    // ProductPricingService is the shared spine behind the catalog-pricing call sites in Buyer.Web
    // and Seller.Web: the IsGrulaConfigured gate, lazy resolution of the client and the price
    // products, the IPriceService call, and index-safe access to the positional response via
    // PricedProducts.ElementAtOrDefault. Both factory delegates are lazy so the gate can short-circuit before either
    // runs - in particular before a resolver's API calls run.
    public class ProductPricingServiceTests
    {
        private sealed class TestPricingSettings : IPricingSettings
        {
            public string GrulaAccessToken { get; set; }
            public string GrulaEnvironmentId { get; set; }
            public string DefaultCurrency { get; set; }
            public string EnablePricesForClients { get; set; }
            public bool IsGrulaConfigured { get; set; }
        }

        private static ProductPricingService CreateService(IPriceService priceService, bool isGrulaConfigured)
        {
            var settings = new TestPricingSettings { IsGrulaConfigured = isGrulaConfigured };
            return new ProductPricingService(priceService, settings, Substitute.For<ILogger<ProductPricingService>>());
        }

        [Fact]
        public async Task GetPricesAsync_WhenGrulaIsNotConfigured_ReturnsEmptyWithoutInvokingEitherFactory()
        {
            var priceService = Substitute.For<IPriceService>();
            var service = CreateService(priceService, isGrulaConfigured: false);

            var productsFactoryCalled = false;
            var clientFactoryCalled = false;

            var prices = await service.GetPricesAsync(
                () => { productsFactoryCalled = true; return Task.FromResult(Enumerable.Empty<PriceProduct>()); },
                () => { clientFactoryCalled = true; return Task.FromResult(new PriceClient()); });

            Assert.True(prices.IsEmpty);
            Assert.False(productsFactoryCalled);
            Assert.False(clientFactoryCalled);
            priceService.DidNotReceive().CanSeePrices(Arg.Any<Guid?>());
        }

        [Fact]
        public async Task GetPricesAsync_WhenCanSeePricesIsFalse_ReturnsEmptyWithoutInvokingThePriceProductFactory()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(false);
            var service = CreateService(priceService, isGrulaConfigured: true);

            var productsFactoryCalled = false;

            var prices = await service.GetPricesAsync(
                () => { productsFactoryCalled = true; return Task.FromResult(Enumerable.Empty<PriceProduct>()); },
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.True(prices.IsEmpty);
            Assert.False(productsFactoryCalled);
        }

        [Fact]
        public async Task GetPricesAsync_HappyPath_ReturnsOnePricePerProductInOrder()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);

            var priceProducts = new List<PriceProduct>
            {
                new() { PrimarySku = "A", IsOutlet = "No" },
                new() { PrimarySku = "B", IsOutlet = "No" }
            };
            var expectedPrices = new List<Price>
            {
                new() { CurrentPrice = 10m, CurrencyCode = "EUR" },
                new() { CurrentPrice = 20m, CurrencyCode = "EUR" }
            };

            priceService.GetPrices(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IEnumerable<Price>>(expectedPrices));

            var service = CreateService(priceService, isGrulaConfigured: true);

            var prices = await service.GetPricesAsync(
                () => Task.FromResult<IEnumerable<PriceProduct>>(priceProducts),
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.False(prices.IsEmpty);
            Assert.Equal(10m, prices.ElementAtOrDefault(0).CurrentPrice);
            Assert.Equal(20m, prices.ElementAtOrDefault(1).CurrentPrice);
        }

        [Fact]
        public async Task GetPricesAsync_WhenGrulaReturnsFewerPricesThanProducts_ReturnsEmptyRatherThanMisassigningByIndex()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);

            var priceProducts = new List<PriceProduct>
            {
                new() { PrimarySku = "A", IsOutlet = "No" },
                new() { PrimarySku = "B", IsOutlet = "No" },
                new() { PrimarySku = "C", IsOutlet = "No" }
            };

            priceService.GetPrices(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IEnumerable<Price>>(new[]
                {
                    new Price { CurrentPrice = 10m, CurrencyCode = "EUR" },
                    new Price { CurrentPrice = 20m, CurrencyCode = "EUR" }
                }));

            var service = CreateService(priceService, isGrulaConfigured: true);

            var prices = await service.GetPricesAsync(
                () => Task.FromResult<IEnumerable<PriceProduct>>(priceProducts),
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.True(prices.IsEmpty);
            Assert.Null(prices.ElementAtOrDefault(0));
        }

        [Fact]
        public async Task GetPricesAsync_WhenProductCollectionIsEmpty_ReturnsEmptyWithoutCallingThePriceService()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            var service = CreateService(priceService, isGrulaConfigured: true);

            var prices = await service.GetPricesAsync(
                () => Task.FromResult(Enumerable.Empty<PriceProduct>()),
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.True(prices.IsEmpty);
            await priceService.DidNotReceive().GetPrices(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public async Task GetPricesAsync_WhenProductCollectionIsNull_ReturnsEmptyWithoutCallingThePriceService()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            var service = CreateService(priceService, isGrulaConfigured: true);

            var prices = await service.GetPricesAsync(
                () => Task.FromResult<IEnumerable<PriceProduct>>(null),
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.True(prices.IsEmpty);
            await priceService.DidNotReceive().GetPrices(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public void At_WithOutOfRangeIndexes_ReturnsNullWithoutThrowing()
        {
            var prices = new PricedProducts(new List<Price> { new() { CurrentPrice = 1m } });

            Assert.Null(prices.ElementAtOrDefault(-1));
            Assert.Null(prices.ElementAtOrDefault(1));
        }

        [Fact]
        public async Task GetPriceAsync_WhenGrulaIsNotConfigured_ReturnsNull()
        {
            var priceService = Substitute.For<IPriceService>();
            var service = CreateService(priceService, isGrulaConfigured: false);

            var price = await service.GetPriceAsync(
                () => Task.FromResult(new PriceProduct { IsOutlet = "No" }),
                () => Task.FromResult(new PriceClient()));

            Assert.Null(price);
        }

        [Fact]
        public async Task GetPriceAsync_HappyPath_ReturnsThePrice()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPrice(Arg.Any<DateTime>(), Arg.Any<PriceProduct>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult(new Price { CurrentPrice = 42m, CurrencyCode = "EUR" }));

            var service = CreateService(priceService, isGrulaConfigured: true);

            var price = await service.GetPriceAsync(
                () => Task.FromResult(new PriceProduct { PrimarySku = "A", IsOutlet = "No" }),
                () => Task.FromResult(new PriceClient { Id = Guid.NewGuid() }));

            Assert.Equal(42m, price.CurrentPrice);
        }
    }
}
