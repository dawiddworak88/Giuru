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

    public enum GrulaPriceStatus
    {
        Priced,
        AuthoritativeNoPrice,
        ServiceUnavailable,
        MissingResponse,
        InvalidPriceDrivers
    }

    public sealed record GrulaPrice(decimal? CurrentPrice, string CurrencyCode, GrulaPriceStatus Status = GrulaPriceStatus.Priced);

    /// <summary>
    /// Prices arrive on the request from the browser, so the basket may only be saved with
    /// prices that came back from Grula for that exact line. Both the buyer and the seller
    /// carry their own copy of this code, so both are held to the same rules here.
    /// </summary>
    public abstract class BasketPricingTests
    {
        protected abstract (bool Applied, IList<BasketLine> Lines) ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices);

        protected abstract IList<BasketLine> ClearUntrustedPrices(IList<BasketLine> lines);

        protected abstract string GetOutletPriceDriver(double outletQuantity);

        [Fact]
        public void ClearUntrustedPrices_WhenGrulaIsNotConfigured_ClearsAMaliciousSubmittedPrice()
        {
            var lines = ClearUntrustedPrices(new List<BasketLine>
            {
                new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 0.01m, Price: 0.01m, Currency: "FAKE")
            });

            var line = lines.Single();
            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);
        }

        [Fact]
        public void ClearUntrustedPrices_WhenGrulaIsNotConfigured_ClearsEveryLine()
        {
            var lines = ClearUntrustedPrices(new List<BasketLine>
            {
                new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 0.01m, Price: 0.01m, Currency: "FAKE"),
                new(Quantity: 2, StockQuantity: 3, OutletQuantity: 4, UnitPrice: 999m, Price: 8991m, Currency: "USD")
            });

            Assert.All(lines, line =>
            {
                Assert.Null(line.UnitPrice);
                Assert.Null(line.Price);
                Assert.Null(line.Currency);
            });
        }

        [Fact]
        public void ClearUntrustedPrices_WhenGrulaIsNotConfigured_IsNullAndEmptySafe()
        {
            Assert.Null(ClearUntrustedPrices(null));
            Assert.Empty(ClearUntrustedPrices(new List<BasketLine>()));
        }

        [Fact]
        public void ApplyPrices_WhenGrulaPricedTheLine_OverwritesTheSubmittedPriceAndTotalsByWholeQuantity()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    // The caller submitted a price of its own choosing - it must not survive.
                    new(Quantity: 2, StockQuantity: 3, OutletQuantity: 5, UnitPrice: 0.01m, Price: 0.01m, Currency: "PLN")
                },
                new List<GrulaPrice> { new(CurrentPrice: 12.5m, CurrencyCode: "EUR") });

            Assert.True(result.Applied);
            var line = result.Lines.Single();

            Assert.Equal(12.5m, line.UnitPrice);
            Assert.Equal(125m, line.Price);
            Assert.Equal("EUR", line.Currency);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaAuthoritativelyReturnsNoPrice_ClearsTheSubmittedPrice()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice> { new(CurrentPrice: null, CurrencyCode: null, GrulaPriceStatus.AuthoritativeNoPrice) });

            Assert.True(result.Applied);
            var line = result.Lines.Single();

            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaIsUnavailable_ClearsEverySubmittedPriceAndAllowsSaving()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR"),
                    new(Quantity: 4, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice>
                {
                    new(CurrentPrice: 10m, CurrencyCode: "EUR"),
                    new(CurrentPrice: null, CurrencyCode: null, GrulaPriceStatus.ServiceUnavailable)
                });

            Assert.True(result.Applied);
            Assert.Equal(10m, result.Lines[0].UnitPrice);
            Assert.Equal(10m, result.Lines[0].Price);
            Assert.Equal("EUR", result.Lines[0].Currency);
            AssertLineIsUnpriced(result.Lines[1]);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaResponseIsMissing_ClearsEverySubmittedPriceAndAllowsSaving()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                null);

            Assert.True(result.Applied);
            Assert.All(result.Lines, AssertLineIsUnpriced);
        }

        [Fact]
        public void ApplyPrices_WhenGrulaResponseIsShort_ClearsEverySubmittedPriceAndAllowsSaving()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR"),
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice> { new(CurrentPrice: 10m, CurrencyCode: "EUR") });

            Assert.True(result.Applied);
            Assert.All(result.Lines, AssertLineIsUnpriced);
        }

        [Fact]
        public void ApplyPrices_WhenAResultIsMissingOrMalformed_ClearsOnlyTheAffectedLine()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 2, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR"),
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR")
                },
                new List<GrulaPrice>
                {
                    new(CurrentPrice: 12m, CurrencyCode: "EUR"),
                    new(CurrentPrice: 25m, CurrencyCode: null, GrulaPriceStatus.Priced)
                });

            Assert.True(result.Applied);
            Assert.Equal(12m, result.Lines[0].UnitPrice);
            Assert.Equal(24m, result.Lines[0].Price);
            AssertLineIsUnpriced(result.Lines[1]);
        }

        [Fact]
        public void ApplyPrices_WhenPriceDriversAreInvalid_DoesNotPartiallyMutateLines()
        {
            var result = ApplyPrices(
                new List<BasketLine>
                {
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 999m, Price: 999m, Currency: "EUR"),
                    new(Quantity: 1, StockQuantity: 0, OutletQuantity: 0, UnitPrice: 888m, Price: 888m, Currency: "USD")
                },
                new List<GrulaPrice>
                {
                    new(CurrentPrice: 10m, CurrencyCode: "EUR"),
                    new(CurrentPrice: null, CurrencyCode: null, GrulaPriceStatus.InvalidPriceDrivers)
                });

            Assert.False(result.Applied);
            Assert.Equal(999m, result.Lines[0].UnitPrice);
            Assert.Equal(999m, result.Lines[0].Price);
            Assert.Equal("EUR", result.Lines[0].Currency);
            Assert.Equal(888m, result.Lines[1].UnitPrice);
            Assert.Equal(888m, result.Lines[1].Price);
            Assert.Equal("USD", result.Lines[1].Currency);
        }

        [Fact]
        public void ApplyPrices_WhenThereAreNoLines_DoesNotThrow()
        {
            var result = ApplyPrices(null, new List<GrulaPrice> { new(CurrentPrice: 10m, CurrencyCode: "EUR") });
            Assert.True(result.Applied);
            Assert.Null(result.Lines);
        }

        private static void AssertLineIsUnpriced(BasketLine line)
        {
            Assert.Null(line.UnitPrice);
            Assert.Null(line.Price);
            Assert.Null(line.Currency);
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
        protected override (bool Applied, IList<BasketLine> Lines) ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices)
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

            var applied = BuyerBasketsApiController.ApplyPrices(
                basketItems,
                prices?.Select(x => x is null ? null : new Buyer.Web.Shared.DomainModels.Prices.PriceLookupResult
                {
                    Status = x.Status switch
                    {
                        GrulaPriceStatus.Priced => Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.Priced,
                        GrulaPriceStatus.AuthoritativeNoPrice => Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.AuthoritativeNoPrice,
                        GrulaPriceStatus.ServiceUnavailable => Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.ServiceUnavailable,
                        GrulaPriceStatus.MissingResponse => Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.MissingResponse,
                        GrulaPriceStatus.InvalidPriceDrivers => Buyer.Web.Shared.DomainModels.Prices.PriceLookupStatus.InvalidPriceDrivers,
                        _ => throw new System.ArgumentOutOfRangeException()
                    },
                    Price = x.CurrentPrice.HasValue ? new BuyerPrice { CurrentPrice = x.CurrentPrice.Value, CurrencyCode = x.CurrencyCode } : null
                }).ToList());

            return (applied, basketItems?
                .Select(x => new BasketLine(x.Quantity, x.StockQuantity, x.OutletQuantity, x.UnitPrice, x.Price, x.Currency))
                .ToList());
        }

        protected override IList<BasketLine> ClearUntrustedPrices(IList<BasketLine> lines)
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

            BuyerBasketsApiController.ClearUntrustedPrices(basketItems);

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
        protected override (bool Applied, IList<BasketLine> Lines) ApplyPrices(IList<BasketLine> lines, IList<GrulaPrice> prices)
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

            var applied = SellerBasketsApiController.ApplyPrices(
                basketItems,
                prices?.Select(x => x is null ? null : new Seller.Web.Shared.DomainModels.Prices.PriceLookupResult
                {
                    Status = x.Status switch
                    {
                        GrulaPriceStatus.Priced => Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.Priced,
                        GrulaPriceStatus.AuthoritativeNoPrice => Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.AuthoritativeNoPrice,
                        GrulaPriceStatus.ServiceUnavailable => Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.ServiceUnavailable,
                        GrulaPriceStatus.MissingResponse => Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.MissingResponse,
                        GrulaPriceStatus.InvalidPriceDrivers => Seller.Web.Shared.DomainModels.Prices.PriceLookupStatus.InvalidPriceDrivers,
                        _ => throw new System.ArgumentOutOfRangeException()
                    },
                    Price = x.CurrentPrice.HasValue ? new SellerPrice { CurrentPrice = x.CurrentPrice.Value, CurrencyCode = x.CurrencyCode } : null
                }).ToList());

            return (applied, basketItems?
                .Select(x => new BasketLine(x.Quantity, x.StockQuantity, x.OutletQuantity, x.UnitPrice, x.Price, x.Currency))
                .ToList());
        }

        protected override IList<BasketLine> ClearUntrustedPrices(IList<BasketLine> lines)
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

            SellerBasketsApiController.ClearUntrustedPrices(basketItems);

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
