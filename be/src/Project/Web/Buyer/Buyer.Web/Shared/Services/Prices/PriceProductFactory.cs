using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Foundation.Pricing.DomainModels;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    public class PriceProductFactory : IPriceProductFactory
    {
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;
        private readonly IOptions<AppSettings> _options;

        public PriceProductFactory(
            IProductsService productsService,
            IProductColorsService productColorsService,
            IOptions<AppSettings> options)
        {
            _productsService = productsService;
            _productColorsService = productColorsService;
            _options = options;
        }

        public Task<PriceProduct> CreateAsync(Product product, bool isOutletPurchase)
        {
            return PriceProductBuilder.BuildAsync(
                product.PrimaryProductSku,
                product.Sku,
                new ProductAttributeReader(_productsService, product.ProductAttributes),
                new ProductColorTranslator(_productColorsService),
                _options.Value,
                isOutletPurchase);
        }

        public async Task<IEnumerable<PriceProduct>> CreateAsync(IEnumerable<Product> products, bool isOutletPurchase)
        {
            var priceProducts = products.OrEmptyIfNull().Select(x => CreateAsync(x, isOutletPurchase));

            return await Task.WhenAll(priceProducts);
        }

        public PriceProduct Create(CatalogItemViewModel product, bool isOutletPurchase)
        {
            return new PriceProduct
            {
                PrimarySku = product.PrimaryProductSku,
                ProductVariantSku = product.Sku,
                FabricsGroup = product.FabricsGroup,
                SleepAreaSize = product.SleepAreaSize,
                ExtraPacking = product.ExtraPacking,
                PaletteSize = product.PaletteSize,
                Size = product.Size,
                PointsOfLight = product.PointsOfLight,
                LampshadeType = product.LampshadeType,
                LampshadeSize = product.LampshadeSize,
                LinearLight = product.LinearLight,
                Mirror = product.Mirror,
                Shape = product.Shape,
                PrimaryColor = product.PrimaryColor,
                SecondaryColor = product.SecondaryColor,
                BodyColour = product.BodyColour,
                ShelfType = product.ShelfType,
                NumberOfMirrors = product.NumberOfMirrors,
                Led = product.Led,
                IsOutlet = isOutletPurchase.ToYesOrNo()
            };
        }

        private sealed class ProductAttributeReader : IProductAttributeReader
        {
            private readonly IProductsService _productsService;
            private readonly IEnumerable<ProductAttribute> _attributes;

            public ProductAttributeReader(IProductsService productsService, IEnumerable<ProductAttribute> attributes)
            {
                _productsService = productsService;
                _attributes = attributes;
            }

            public string GetFirstAvailableAttributeValue(string possibleKeys) =>
                _productsService.GetFirstAvailableAttributeValue(_attributes, possibleKeys);

            public string GetSleepAreaSize() => _productsService.GetSleepAreaSize(_attributes);

            public string GetSize() => _productsService.GetSize(_attributes);
        }

        private sealed class ProductColorTranslator : IProductColorTranslator
        {
            private readonly IProductColorsService _productColorsService;

            public ProductColorTranslator(IProductColorsService productColorsService)
            {
                _productColorsService = productColorsService;
            }

            public Task<string> ToEnglishAsync(string color) => _productColorsService.ToEnglishAsync(color);
        }
    }
}
