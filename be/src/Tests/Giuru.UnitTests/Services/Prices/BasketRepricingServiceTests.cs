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
    // BasketRepricingService is the shared spine behind BasketsApiController.RepriceBasketItemsAsync
    // and OrderFileApiController.RepriceMergedBasketAsync in both apps: index the already-resolved
    // lines, build PriceProducts, ask IPriceService, align the response back onto the lines by index,
    // and apply it. SKU resolution and the "unresolved SKU" warnings stay the caller's
    // responsibility, so this only ever sees lines whose SKU already resolves - or does not - in the
    // supplied product lookup.
    public class BasketRepricingServiceTests
    {
        private sealed class FakeProduct
        {
            public string Sku { get; init; }
        }

        private sealed class FakeBasketLine : IPriceableBasketLine
        {
            public string Sku { get; init; }
            public double Quantity { get; init; }
            public double StockQuantity { get; init; }
            public double OutletQuantity { get; init; }
            public decimal? UnitPrice { get; set; }
            public decimal? Price { get; set; }
            public string Currency { get; set; }
        }

        [Fact]
        public async Task RepriceAsync_WhenGrulaPricesEveryResolvedLine_AppliesThePriceAndTotalsByWholeQuantity()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 12.5m, CurrencyCode = "EUR" } }
                }));

            var service = new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
            var lines = new List<FakeBasketLine> { new() { Sku = "RESOLVED", Quantity = 2, StockQuantity = 3, OutletQuantity = 0 } };
            var productLookup = new Dictionary<string, FakeProduct> { ["RESOLVED"] = new() { Sku = "RESOLVED" } };

            var outcome = await service.RepriceAsync(
                lines,
                line => line.Sku,
                line => line.OutletQuantity > 0,
                productLookup,
                (product, isOutlet) => Task.FromResult(new PriceProduct { PrimarySku = product.Sku, ProductVariantSku = product.Sku, IsOutlet = isOutlet ? "Yes" : "No" }),
                new PriceClient(),
                DateTime.UtcNow);

            Assert.True(outcome.Succeeded);
            var line = Assert.Single(lines);
            Assert.Equal(12.5m, line.UnitPrice);
            Assert.Equal(62.5m, line.Price);
            Assert.Equal("EUR", line.Currency);
        }

        [Fact]
        public async Task RepriceAsync_WhenALineSkuDoesNotResolveInTheProductLookup_LeavesThatLineOutOfThePriceRequest()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 12m, CurrencyCode = "EUR" } }
                }));

            var service = new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
            var lines = new List<FakeBasketLine>
            {
                new() { Sku = "RESOLVED", Quantity = 1 },
                new() { Sku = "UNKNOWN", Quantity = 1 }
            };
            var productLookup = new Dictionary<string, FakeProduct> { ["RESOLVED"] = new() { Sku = "RESOLVED" } };

            var outcome = await service.RepriceAsync(
                lines,
                line => line.Sku,
                line => false,
                productLookup,
                (product, isOutlet) => Task.FromResult(new PriceProduct { PrimarySku = product.Sku, ProductVariantSku = product.Sku, IsOutlet = isOutlet ? "Yes" : "No" }),
                new PriceClient(),
                DateTime.UtcNow);

            Assert.True(outcome.Succeeded);

            var pricedProducts = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[1] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("RESOLVED", priceProduct.ProductVariantSku);

            var resolved = lines.Single(x => x.Sku == "RESOLVED");
            Assert.Equal(12m, resolved.UnitPrice);
            var unresolved = lines.Single(x => x.Sku == "UNKNOWN");
            Assert.Null(unresolved.UnitPrice);
        }

        [Fact]
        public async Task RepriceAsync_WhenGrulaReturnsFewerResultsThanPricedLines_PersistsTheBasketUnpricedAndSucceeds()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(Array.Empty<PriceLookupResult>()));

            var logger = Substitute.For<ILogger<BasketRepricingService>>();
            var service = new BasketRepricingService(priceService, logger);
            var lines = new List<FakeBasketLine> { new() { Sku = "RESOLVED", Quantity = 1, UnitPrice = 999m, Price = 999m, Currency = "FAKE" } };
            var productLookup = new Dictionary<string, FakeProduct> { ["RESOLVED"] = new() { Sku = "RESOLVED" } };

            var outcome = await service.RepriceAsync(
                lines,
                line => line.Sku,
                line => false,
                productLookup,
                (product, isOutlet) => Task.FromResult(new PriceProduct { PrimarySku = product.Sku, ProductVariantSku = product.Sku, IsOutlet = isOutlet ? "Yes" : "No" }),
                new PriceClient(),
                DateTime.UtcNow);

            Assert.True(outcome.Succeeded);
            var line = Assert.Single(lines);
            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);
        }

        [Fact]
        public async Task RepriceAsync_WhenGrulaReturnsAnUnrecognisedStatus_FailsClosedWithoutApplyingAnyPrice()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = (PriceLookupStatus)(-1) }
                }));

            var service = new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
            var lines = new List<FakeBasketLine> { new() { Sku = "RESOLVED", Quantity = 1 } };
            var productLookup = new Dictionary<string, FakeProduct> { ["RESOLVED"] = new() { Sku = "RESOLVED" } };

            var outcome = await service.RepriceAsync(
                lines,
                line => line.Sku,
                line => false,
                productLookup,
                (product, isOutlet) => Task.FromResult(new PriceProduct { PrimarySku = product.Sku, ProductVariantSku = product.Sku, IsOutlet = isOutlet ? "Yes" : "No" }),
                new PriceClient(),
                DateTime.UtcNow);

            Assert.False(outcome.Succeeded);
        }

        [Fact]
        public async Task RepriceAsync_PassesTheSuppliedDiscountCodeAndPricingDateThroughUnchanged()
        {
            var priceService = Substitute.For<IPriceService>();
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 1m, CurrencyCode = "EUR" } }
                }));

            var service = new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
            var lines = new List<FakeBasketLine> { new() { Sku = "RESOLVED", Quantity = 1 } };
            var productLookup = new Dictionary<string, FakeProduct> { ["RESOLVED"] = new() { Sku = "RESOLVED" } };
            var priceClient = new PriceClient { DiscountCode = "SUMMER25" };
            var pricingDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            await service.RepriceAsync(
                lines,
                line => line.Sku,
                line => false,
                productLookup,
                (product, isOutlet) => Task.FromResult(new PriceProduct { PrimarySku = product.Sku, ProductVariantSku = product.Sku, IsOutlet = isOutlet ? "Yes" : "No" }),
                priceClient,
                pricingDate);

            await priceService.Received(1).GetPriceResultsForBasketAsync(pricingDate, Arg.Any<IEnumerable<PriceProduct>>(), priceClient);
        }
    }
}
