using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BuyerAppSettings = Buyer.Web.Shared.Configurations.AppSettings;
using BuyerPriceService = Buyer.Web.Shared.Services.Prices.PriceService;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerPriceService = Seller.Web.Shared.Services.Prices.PriceService;

namespace Giuru.UnitTests.Orders.Baskets
{
    /// <summary>
    /// CanSeePrices never touches the Grula client, so a real GrulaApiClient instance is
    /// constructed purely to satisfy PriceService's constructor - it is never called.
    /// </summary>
    public abstract class CanSeePricesTests
    {
        protected abstract bool CanSeePrices(string enablePricesForClients, Guid? priceClientId);
        protected abstract Task<IReadOnlyList<GrulaPriceResult>> GetPriceResultsForBasketAsync(string enablePricesForClients, Guid? priceClientId, int productCount);

        [Fact]
        public void CanSeePrices_WhenAllowListIsBlank_ReturnsTrueForAnyClient()
        {
            Assert.True(CanSeePrices(null, Guid.NewGuid()));
        }

        [Fact]
        public void CanSeePrices_WhenAllowListIsBlank_ReturnsTrueForNullClient()
        {
            Assert.True(CanSeePrices(string.Empty, null));
        }

        [Fact]
        public void CanSeePrices_WhenClientIsInAllowList_ReturnsTrue()
        {
            var clientId = Guid.NewGuid();
            var allowList = $"{Guid.NewGuid()}&{clientId}&{Guid.NewGuid()}";

            Assert.True(CanSeePrices(allowList, clientId));
        }

        [Fact]
        public void CanSeePrices_WhenClientIsNotInAllowList_ReturnsFalse()
        {
            var allowList = $"{Guid.NewGuid()}&{Guid.NewGuid()}";

            Assert.False(CanSeePrices(allowList, Guid.NewGuid()));
        }

        [Fact]
        public void CanSeePrices_WhenAllowListIsConfiguredAndClientIsNull_ReturnsFalse()
        {
            var allowList = $"{Guid.NewGuid()}";

            Assert.False(CanSeePrices(allowList, null));
        }

        [Fact]
        public async Task GetPriceResultsForBasketAsync_WhenClientIsOutsideAllowList_ReturnsPricesHiddenForEveryProduct()
        {
            var results = await GetPriceResultsForBasketAsync(Guid.NewGuid().ToString(), Guid.NewGuid(), productCount: 2);

            Assert.Equal(2, results.Count);
            Assert.All(results, result =>
            {
                Assert.Equal(PriceResultStatus.PricesHidden, result.Status);
                Assert.Null(result.Price);
            });
        }
    }

    public enum PriceResultStatus
    {
        PricesHidden
    }

    public sealed record GrulaPriceResult(PriceResultStatus Status, decimal? Price);

    public class BuyerCanSeePricesTests : CanSeePricesTests
    {
        protected override bool CanSeePrices(string enablePricesForClients, Guid? priceClientId)
        {
            var grulaApiClient = new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient());
            var options = Options.Create(new BuyerAppSettings { EnablePricesForClients = enablePricesForClients });
            var priceService = new BuyerPriceService(grulaApiClient, options, NullLogger<BuyerPriceService>.Instance);

            return priceService.CanSeePrices(priceClientId);
        }

        protected override async Task<IReadOnlyList<GrulaPriceResult>> GetPriceResultsForBasketAsync(string enablePricesForClients, Guid? priceClientId, int productCount)
        {
            var priceService = new BuyerPriceService(
                new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient()),
                Options.Create(new BuyerAppSettings
                {
                    GrulaAccessToken = "test-token",
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    EnablePricesForClients = enablePricesForClients
                }),
                NullLogger<BuyerPriceService>.Instance);

            var results = await priceService.GetPriceResultsForBasketAsync(
                "token",
                DateTime.UtcNow,
                new[]
                {
                    new Buyer.Web.Shared.DomainModels.Prices.PriceProduct { IsOutlet = "No" },
                    new Buyer.Web.Shared.DomainModels.Prices.PriceProduct { IsOutlet = "No" }
                },
                new Buyer.Web.Shared.DomainModels.Prices.PriceClient { Id = priceClientId });

            return results.Select(x => new GrulaPriceResult(
                x.Status == Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.PricesHidden
                    ? PriceResultStatus.PricesHidden
                    : throw new InvalidOperationException($"Unexpected pricing status: {x.Status}"),
                x.Price?.CurrentPrice)).ToList();
        }
    }

    public class SellerCanSeePricesTests : CanSeePricesTests
    {
        protected override bool CanSeePrices(string enablePricesForClients, Guid? priceClientId)
        {
            var grulaApiClient = new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient());
            var options = Options.Create(new SellerAppSettings { EnablePricesForClients = enablePricesForClients });
            var priceService = new SellerPriceService(grulaApiClient, options, NullLogger<SellerPriceService>.Instance);

            return priceService.CanSeePrices(priceClientId);
        }

        protected override async Task<IReadOnlyList<GrulaPriceResult>> GetPriceResultsForBasketAsync(string enablePricesForClients, Guid? priceClientId, int productCount)
        {
            var priceService = new SellerPriceService(
                new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient()),
                Options.Create(new SellerAppSettings
                {
                    GrulaAccessToken = "test-token",
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    EnablePricesForClients = enablePricesForClients
                }),
                NullLogger<SellerPriceService>.Instance);

            var results = await priceService.GetPriceResultsForBasketAsync(
                DateTime.UtcNow,
                new[]
                {
                    new Seller.Web.Shared.DomainModels.Prices.PriceProduct { IsOutlet = "No" },
                    new Seller.Web.Shared.DomainModels.Prices.PriceProduct { IsOutlet = "No" }
                },
                new Seller.Web.Shared.DomainModels.Prices.PriceClient { Id = priceClientId });

            return results.Select(x => new GrulaPriceResult(
                x.Status == Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.PricesHidden
                    ? PriceResultStatus.PricesHidden
                    : throw new InvalidOperationException($"Unexpected pricing status: {x.Status}"),
                x.Price?.CurrentPrice)).ToList();
        }
    }
}
