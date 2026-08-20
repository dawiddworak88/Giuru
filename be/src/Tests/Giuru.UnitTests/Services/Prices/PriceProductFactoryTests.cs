using Microsoft.Extensions.Options;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;
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
    // never omitted, and never derived from product/stock state. See Issues.md #4.
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
}
