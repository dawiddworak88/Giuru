using Microsoft.Extensions.Options;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using BuyerAppSettings = Buyer.Web.Shared.Configurations.AppSettings;
using BuyerIProductColorsService = Buyer.Web.Areas.Products.Services.ProductColors.IProductColorsService;
using BuyerIProductsService = Buyer.Web.Areas.Products.Services.Products.IProductsService;
using BuyerPriceProductFactory = Buyer.Web.Shared.Services.Prices.PriceProductFactory;
using BuyerProduct = Buyer.Web.Areas.Products.DomainModels.Product;
using BuyerCatalogItemViewModel = Buyer.Web.Shared.ViewModels.Catalogs.CatalogItemViewModel;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerIProductColorsService = Seller.Web.Shared.Services.ProductColors.IProductColorsService;
using SellerIProductsService = Seller.Web.Shared.Services.Products.IProductsService;
using SellerPriceProductFactory = Seller.Web.Shared.Services.Prices.PriceProductFactory;
using SellerProduct = Seller.Web.Areas.Products.DomainModels.Product;

namespace Giuru.UnitTests.Services.Prices
{
    // The Outlet price driver must express the line's purchase intent, sent on every call -
    // never omitted, and never derived from product/stock state, or the catalog and the basket
    // quote the same line at different prices.
    // The 20-property attribute -> PriceProduct mapping lives once in PriceProductBuilder and is
    // pinned by PriceProductBuilderTests below. What is left app-specific, and therefore still
    // tested per app, is only the thin adapter: does the outlet flag reach the mapping, and are
    // the two SKU fields wired correctly.
    public class BuyerPriceProductFactoryTests
    {
        [Theory]
        [InlineData(false, "No")]
        [InlineData(true, "Yes")]
        public async Task CreateAsync_FromProduct_SetsIsOutletFromThePurchaseIntentArgument(bool isOutletPurchase, string expected)
        {
            var factory = new BuyerPriceProductFactory(
                Substitute.For<BuyerIProductsService>(),
                Substitute.For<BuyerIProductColorsService>(),
                Options.Create(new BuyerAppSettings()));

            var priceProduct = await factory.CreateAsync(
                new BuyerProduct { PrimaryProductSku = "PRIMARY", Sku = "SKU" },
                isOutletPurchase);

            Assert.Equal(expected, priceProduct.IsOutlet);
            Assert.Equal("PRIMARY", priceProduct.PrimarySku);
            Assert.Equal("SKU", priceProduct.ProductVariantSku);
        }

        [Fact]
        public async Task CreateAsync_FromProductCollection_AppliesTheSamePurchaseIntentToEveryLine()
        {
            var factory = new BuyerPriceProductFactory(
                Substitute.For<BuyerIProductsService>(),
                Substitute.For<BuyerIProductColorsService>(),
                Options.Create(new BuyerAppSettings()));

            var priceProducts = await factory.CreateAsync(
                new[]
                {
                    new BuyerProduct { PrimaryProductSku = "A", Sku = "A" },
                    new BuyerProduct { PrimaryProductSku = "B", Sku = "B" }
                },
                isOutletPurchase: true);

            Assert.All(priceProducts, x => Assert.Equal("Yes", x.IsOutlet));
        }

        [Theory]
        [InlineData(false, "No")]
        [InlineData(true, "Yes")]
        public void Create_FromCatalogItemViewModel_SetsIsOutletFromThePurchaseIntentArgument(bool isOutletPurchase, string expected)
        {
            var factory = new BuyerPriceProductFactory(
                Substitute.For<BuyerIProductsService>(),
                Substitute.For<BuyerIProductColorsService>(),
                Options.Create(new BuyerAppSettings()));

            var priceProduct = factory.Create(
                new BuyerCatalogItemViewModel { PrimaryProductSku = "PRIMARY", Sku = "SKU", FabricsGroup = "GROUP" },
                isOutletPurchase);

            Assert.Equal(expected, priceProduct.IsOutlet);
            Assert.Equal("PRIMARY", priceProduct.PrimarySku);
            Assert.Equal("GROUP", priceProduct.FabricsGroup);
        }
    }

    public class SellerPriceProductFactoryTests
    {
        [Theory]
        [InlineData(false, "No")]
        [InlineData(true, "Yes")]
        public async Task CreateAsync_FromProduct_SetsIsOutletFromThePurchaseIntentArgument(bool isOutletPurchase, string expected)
        {
            var factory = new SellerPriceProductFactory(
                Substitute.For<SellerIProductsService>(),
                Substitute.For<SellerIProductColorsService>(),
                Options.Create(new SellerAppSettings()));

            var priceProduct = await factory.CreateAsync(
                new SellerProduct { PrimaryProductSku = "PRIMARY", Sku = "SKU" },
                isOutletPurchase);

            Assert.Equal(expected, priceProduct.IsOutlet);
            Assert.Equal("PRIMARY", priceProduct.PrimarySku);
            Assert.Equal("SKU", priceProduct.ProductVariantSku);
        }

        [Fact]
        public async Task CreateAsync_FromProductCollection_AppliesTheSamePurchaseIntentToEveryLine()
        {
            var factory = new SellerPriceProductFactory(
                Substitute.For<SellerIProductsService>(),
                Substitute.For<SellerIProductColorsService>(),
                Options.Create(new SellerAppSettings()));

            var priceProducts = await factory.CreateAsync(
                new[]
                {
                    new SellerProduct { PrimaryProductSku = "A", Sku = "A" },
                    new SellerProduct { PrimaryProductSku = "B", Sku = "B" }
                },
                isOutletPurchase: false);

            Assert.All(priceProducts, x => Assert.Equal("No", x.IsOutlet));
        }
    }

    // Golden-master guard rail: pins the attribute -> PriceProduct mapping (every
    // Possible*AttributeKeys setting, both colour translations, both sku fields and the outlet
    // flag). It runs once, directly against the shared PriceProductBuilder, using fakes instead of
    // either app's Product/IProductsService types - the point of sharing the mapping was that it no
    // longer needs a Buyer copy and a Seller copy of this test.
    public class PriceProductBuilderTests
    {
        private const string PrimarySku = "PRIMARY-SKU";
        private const string ProductVariantSku = "VARIANT-SKU";
        private const bool IsOutletPurchase = true;

        private const string PriceGroupKey = "PriceGroup.Keys";
        private const string ExtraPackingKey = "ExtraPacking.Keys";
        private const string PaletteSizeKey = "PaletteSize.Keys";
        private const string PointsOfLightKey = "PointsOfLight.Keys";
        private const string LampshadeTypeKey = "LampshadeType.Keys";
        private const string LampshadeSizeKey = "LampshadeSize.Keys";
        private const string LinearLightKey = "LinearLight.Keys";
        private const string MirrorKey = "Mirror.Keys";
        private const string ShapeKey = "Shape.Keys";
        private const string PrimaryColorKey = "PrimaryColor.Keys";
        private const string SecondaryColorKey = "SecondaryColor.Keys";
        private const string BodyColorKey = "BodyColor.Keys";
        private const string ShelfTypeKey = "ShelfType.Keys";
        private const string NumberOfMirrorsKey = "NumberOfMirrors.Keys";
        private const string LedKey = "Led.Keys";

        private const string FabricsGroupRaw = "FABRICS-GROUP-RAW";
        private const string ExtraPackingRaw = "true";
        private const string SleepAreaSizeRaw = "SLEEP-AREA-SIZE-RAW";
        private const string PaletteSizeRaw = "PALETTE-SIZE-RAW";
        private const string SizeRaw = "SIZE-RAW";
        private const string PointsOfLightRaw = "POINTS-OF-LIGHT-RAW";
        private const string LampshadeTypeRaw = "LAMPSHADE-TYPE-RAW";
        private const string LampshadeSizeRaw = "LAMPSHADE-SIZE-RAW";
        private const string LinearLightRaw = "yes";
        private const string MirrorRaw = "tak";
        private const string ShapeRaw = "SHAPE-RAW";
        private const string PrimaryColorRaw = "PRIMARY-COLOR-RAW";
        private const string SecondaryColorRaw = "SECONDARY-COLOR-RAW";
        private const string BodyColorRaw = "BODY-COLOR-RAW";
        private const string ShelfTypeRaw = "SHELF-TYPE-RAW";
        private const string NumberOfMirrorsRaw = "2";
        private const string LedRaw = "ja";

        private const string PrimaryColorTranslated = "Red";
        private const string SecondaryColorTranslated = "Blue";
        private const string BodyColorTranslated = "Green";

        [Fact]
        public async Task BuildAsync_MapsEveryAttributeToTheGoldenMasterPriceProduct()
        {
            var keys = new FakeAttributeKeys
            {
                PossiblePriceGroupAttributeKeys = PriceGroupKey,
                PossibleExtraPackingAttributeKeys = ExtraPackingKey,
                PossiblePaletteSizeAttributeKeys = PaletteSizeKey,
                PossiblePointsOfLightAttributeKeys = PointsOfLightKey,
                PossibleLampshadeTypeAttributeKeys = LampshadeTypeKey,
                PossibleLampshadeSizeAttributeKeys = LampshadeSizeKey,
                PossibleLinearLightAttributeKeys = LinearLightKey,
                PossibleMirrorAttributeKeys = MirrorKey,
                PossibleShapeAttributeKeys = ShapeKey,
                PossiblePrimaryColorAttributeKeys = PrimaryColorKey,
                PossibleSecondaryColorAttributeKeys = SecondaryColorKey,
                PossibleBodyColorAttributeKeys = BodyColorKey,
                PossibleShelfTypeAttributeKeys = ShelfTypeKey,
                PossibleNumberOfMirrorsAttributeKeys = NumberOfMirrorsKey,
                PossibleLedAttributeKeys = LedKey
            };

            var attributes = new FakeAttributeReader(
                new Dictionary<string, string>
                {
                    [PriceGroupKey] = FabricsGroupRaw,
                    [ExtraPackingKey] = ExtraPackingRaw,
                    [PaletteSizeKey] = PaletteSizeRaw,
                    [PointsOfLightKey] = PointsOfLightRaw,
                    [LampshadeTypeKey] = LampshadeTypeRaw,
                    [LampshadeSizeKey] = LampshadeSizeRaw,
                    [LinearLightKey] = LinearLightRaw,
                    [MirrorKey] = MirrorRaw,
                    [ShapeKey] = ShapeRaw,
                    [PrimaryColorKey] = PrimaryColorRaw,
                    [SecondaryColorKey] = SecondaryColorRaw,
                    [BodyColorKey] = BodyColorRaw,
                    [ShelfTypeKey] = ShelfTypeRaw,
                    [NumberOfMirrorsKey] = NumberOfMirrorsRaw,
                    [LedKey] = LedRaw
                },
                sleepAreaSize: SleepAreaSizeRaw,
                size: SizeRaw);

            var colors = new FakeColorTranslator(new Dictionary<string, string>
            {
                [PrimaryColorRaw] = PrimaryColorTranslated,
                [SecondaryColorRaw] = SecondaryColorTranslated,
                [BodyColorRaw] = BodyColorTranslated
            });

            var priceProduct = await PriceProductBuilder.BuildAsync(
                PrimarySku,
                ProductVariantSku,
                attributes,
                colors,
                keys,
                IsOutletPurchase);

            var expected = new PriceProduct
            {
                PrimarySku = PrimarySku,
                ProductVariantSku = ProductVariantSku,
                FabricsGroup = FabricsGroupRaw,
                ExtraPacking = "Yes",
                SleepAreaSize = SleepAreaSizeRaw,
                PaletteSize = PaletteSizeRaw,
                Size = SizeRaw,
                PointsOfLight = PointsOfLightRaw,
                LampshadeType = LampshadeTypeRaw,
                LampshadeSize = LampshadeSizeRaw,
                LinearLight = "Yes",
                Mirror = "Yes",
                Shape = ShapeRaw,
                PrimaryColor = PrimaryColorTranslated,
                SecondaryColor = SecondaryColorTranslated,
                BodyColour = BodyColorTranslated,
                ShelfType = ShelfTypeRaw,
                NumberOfMirrors = NumberOfMirrorsRaw,
                Led = "Yes",
                IsOutlet = "Yes"
            };

            foreach (var property in typeof(PriceProduct).GetProperties())
            {
                var expectedValue = property.GetValue(expected);
                var actualValue = property.GetValue(priceProduct);

                Assert.True(
                    Equals(expectedValue, actualValue),
                    $"{property.Name}: expected '{expectedValue}' but was '{actualValue}'.");
            }
        }

        private sealed class FakeAttributeReader : IProductAttributeReader
        {
            private readonly IReadOnlyDictionary<string, string> _values;
            private readonly string _sleepAreaSize;
            private readonly string _size;

            public FakeAttributeReader(IReadOnlyDictionary<string, string> values, string sleepAreaSize, string size)
            {
                _values = values;
                _sleepAreaSize = sleepAreaSize;
                _size = size;
            }

            public string GetFirstAvailableAttributeValue(string possibleKeys) =>
                _values.TryGetValue(possibleKeys, out var value) ? value : null;

            public string GetSleepAreaSize() => _sleepAreaSize;

            public string GetSize() => _size;
        }

        private sealed class FakeColorTranslator : IProductColorTranslator
        {
            private readonly IReadOnlyDictionary<string, string> _translations;

            public FakeColorTranslator(IReadOnlyDictionary<string, string> translations)
            {
                _translations = translations;
            }

            public Task<string> ToEnglishAsync(string color) =>
                Task.FromResult(color is not null && _translations.TryGetValue(color, out var translated) ? translated : color);
        }

        private sealed class FakeAttributeKeys : IPriceProductAttributeKeys
        {
            public string PossibleExtraPackingAttributeKeys { get; set; }
            public string PossiblePriceGroupAttributeKeys { get; set; }
            public string PossibleSleepAreaWidthAttributeKeys { get; set; }
            public string PossibleSleepAreaDepthAttributeKeys { get; set; }
            public string PossibleDepthAttributeKeys { get; set; }
            public string PossibleWidthAttributeKeys { get; set; }
            public string PossibleLengthAttributeKeys { get; set; }
            public string PossiblePaletteSizeAttributeKeys { get; set; }
            public string PossiblePointsOfLightAttributeKeys { get; set; }
            public string PossibleLampshadeTypeAttributeKeys { get; set; }
            public string PossibleLampshadeSizeAttributeKeys { get; set; }
            public string PossibleLinearLightAttributeKeys { get; set; }
            public string PossibleMirrorAttributeKeys { get; set; }
            public string PossibleShapeAttributeKeys { get; set; }
            public string PossiblePrimaryColorAttributeKeys { get; set; }
            public string PossibleSecondaryColorAttributeKeys { get; set; }
            public string PossibleBodyColorAttributeKeys { get; set; }
            public string PossibleShelfTypeAttributeKeys { get; set; }
            public string PossibleNumberOfMirrorsAttributeKeys { get; set; }
            public string PossibleLedAttributeKeys { get; set; }
        }
    }
}
