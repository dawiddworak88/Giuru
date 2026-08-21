using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Orders.Baskets
{
    /// <summary>
    /// CanSeePrices never touches the Grula client, so a real GrulaApiClient instance is
    /// constructed purely to satisfy PriceService's constructor - it is never called.
    /// </summary>
    public class CanSeePricesTests
    {
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
                Assert.Equal(PriceLookupStatus.PricesHidden, result.Status);
                Assert.Null(result.Price);
            });
        }

        private static bool CanSeePrices(string enablePricesForClients, Guid? priceClientId)
        {
            var grulaApiClient = new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient());
            var settings = new TestPricingSettings { EnablePricesForClients = enablePricesForClients };
            var priceService = new PriceService(grulaApiClient, settings, NullLogger<PriceService>.Instance);

            return priceService.CanSeePrices(priceClientId);
        }

        private static async Task<IReadOnlyList<PriceLookupResult>> GetPriceResultsForBasketAsync(string enablePricesForClients, Guid? priceClientId, int productCount)
        {
            var priceService = new PriceService(
                new Grula.PricingIntelligencePlatform.Sdk.GrulaApiClient("http://localhost", new HttpClient()),
                new TestPricingSettings
                {
                    GrulaAccessToken = "test-token",
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    EnablePricesForClients = enablePricesForClients
                },
                NullLogger<PriceService>.Instance);

            return await priceService.GetPriceResultsForBasketAsync(
                DateTime.UtcNow,
                Enumerable.Range(0, productCount).Select(_ => new PriceProduct { IsOutlet = "No" }),
                new PriceClient { Id = priceClientId });
        }

        private sealed class TestPricingSettings : Foundation.Pricing.Configurations.IPricingSettings
        {
            public string GrulaAccessToken { get; set; }
            public string GrulaEnvironmentId { get; set; }
            public string DefaultCurrency { get; set; }
            public string EnablePricesForClients { get; set; }

            public bool IsGrulaConfigured =>
                !string.IsNullOrWhiteSpace(GrulaAccessToken) && Guid.TryParse(GrulaEnvironmentId, out _);
        }
    }
}
