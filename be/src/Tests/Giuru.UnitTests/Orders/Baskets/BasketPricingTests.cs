using System.Collections.Generic;
using System.Linq;
using BuyerBasketItemRequestModel = Buyer.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using BuyerBasketsApiController = Buyer.Web.Areas.Orders.ApiControllers.BasketsApiController;
using BuyerPrice = Buyer.Web.Shared.DomainModels.Prices.Price;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketsApiController = Seller.Web.Areas.Orders.ApiControllers.BasketsApiController;
using SellerPrice = Seller.Web.Shared.DomainModels.Prices.Price;

namespace Giuru.UnitTests.Orders.Baskets
{
    public sealed record BasketLine(
        double Quantity,
        double StockQuantity,
        double OutletQuantity,
        decimal? UnitPrice,
        decimal? Price,
        string Currency);

    public sealed record GrulaPrice(decimal CurrentPrice, string CurrencyCode);

    /// <summary>
    /// Prices arrive on the request from the browser, so the basket may only be saved with
    /// prices that came back from Grula for that exact line. Both the buyer and the seller
    /// carry their own copy of this code, so both are held to the same rules here.
    /// </summary>
    public abstract class BasketPricingTests
    {
        protected abstract IList<BasketLine> ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices);

        protected abstract string GetOutletPriceDriver(double outletQuantity);

        [Fact]
        public void ApplyPrices_WhenGrulaPricedTheLine_OverwritesTheSubmittedPriceAndTotalsByWholeQuantity()
        {
            var lines = ApplyPrices(
                new List<BasketLine>
                {
                    // The caller submitted a price of its own choosing - it must not survive.
                    new(Quantity: 2, StockQuantity: 3, OutletQuantity: 5, UnitPrice: 0.01m, Price: 0.01m, Currency: "PLN")
                },
                new List<GrulaPrice> { new(CurrentPrice: 12.5m, CurrencyCode: "EUR") });

            var line = lines.Single();

            Assert.Equal(12.5m, line.UnitPrice);
            Assert.Equal(125m, line.Price);
            Assert.Equal("EUR", line.Currency);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaDidNotPriceTheLine_ClearsTheSubmittedPrice()
        {
            var lines = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice> { null });

            var line = lines.Single();

            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);
        }

        [Fact]
        public void ApplyPrices_WhenFewerPricesThanLines_ClearsTheSubmittedPriceOnTheUnpricedLines()
        {
            // Lines without a catalog product never make it into the Grula call, so the
            // aligned array is shorter than the basket.
            var lines = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR"),
                    new(Quantity: 4, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice> { new(CurrentPrice: 10m, CurrencyCode: "EUR") });

            Assert.Equal(10m, lines[0].UnitPrice);
            Assert.Equal(10m, lines[0].Price);
            Assert.Equal("EUR", lines[0].Currency);

            Assert.Null(lines[1].UnitPrice);
            Assert.Null(lines[1].Price);
            Assert.Null(lines[1].Currency);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaReturnedNothing_ClearsEverySubmittedPrice()
        {
            var lines = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                null);

            Assert.All(lines, line =>
            {
                Assert.Null(line.UnitPrice);
                Assert.Null(line.Price);
                Assert.Null(line.Currency);
            });
        }

        [Fact]
        public void ApplyPrices_WhenThereAreNoLines_DoesNotThrow()
        {
            Assert.Null(ApplyPrices(null, new List<GrulaPrice> { new(CurrentPrice: 10m, CurrencyCode: "EUR") }));
        }

        [Theory]
        [InlineData(0, "No")]
        [InlineData(1, "Yes")]
        [InlineData(2.5, "Yes")]
        public void GetOutletPriceDriver_MarksTheLineAsOutletOnlyWhenItHasOutletQuantity(double outletQuantity, string expected)
        {
            Assert.Equal(expected, GetOutletPriceDriver(outletQuantity));
        }
    }

    public class BuyerBasketPricingTests : BasketPricingTests
    {
        protected override IList<BasketLine> ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices)
        {
            var basketItems = lines?.Select(x => new BuyerBasketItemRequestModel
            {
                Quantity = x.Quantity,
                StockQuantity = x.StockQuantity,
                OutletQuantity = x.OutletQuantity,
                UnitPrice = x.UnitPrice,
                Price = x.Price,
                Currency = x.Currency
            }).ToList();

            BuyerBasketsApiController.ApplyPrices(
                basketItems,
                prices?.Select(x => x is null ? null : new BuyerPrice { CurrentPrice = x.CurrentPrice, CurrencyCode = x.CurrencyCode }).ToList());

            return basketItems?
                .Select(x => new BasketLine(x.Quantity, x.StockQuantity, x.OutletQuantity, x.UnitPrice, x.Price, x.Currency))
                .ToList();
        }

        protected override string GetOutletPriceDriver(double outletQuantity)
        {
            return BuyerBasketsApiController.GetOutletPriceDriver(new BuyerBasketItemRequestModel { OutletQuantity = outletQuantity });
        }
    }

    public class SellerBasketPricingTests : BasketPricingTests
    {
        protected override IList<BasketLine> ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices)
        {
            var basketItems = lines?.Select(x => new SellerBasketItemRequestModel
            {
                Quantity = x.Quantity,
                StockQuantity = x.StockQuantity,
                OutletQuantity = x.OutletQuantity,
                UnitPrice = x.UnitPrice,
                Price = x.Price,
                Currency = x.Currency
            }).ToList();

            SellerBasketsApiController.ApplyPrices(
                basketItems,
                prices?.Select(x => x is null ? null : new SellerPrice { CurrentPrice = x.CurrentPrice, CurrencyCode = x.CurrencyCode }).ToList());

            return basketItems?
                .Select(x => new BasketLine(x.Quantity, x.StockQuantity, x.OutletQuantity, x.UnitPrice, x.Price, x.Currency))
                .ToList();
        }

        protected override string GetOutletPriceDriver(double outletQuantity)
        {
            return SellerBasketsApiController.GetOutletPriceDriver(new SellerBasketItemRequestModel { OutletQuantity = outletQuantity });
        }
    }
}
