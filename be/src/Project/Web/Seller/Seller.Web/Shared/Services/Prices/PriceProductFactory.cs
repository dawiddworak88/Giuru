using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Prices;
using Seller.Web.Shared.Services.ProductColors;
using Seller.Web.Shared.Services.Products;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Prices
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

        public async Task<PriceProduct> CreateAsync(Product product, bool isOutletPurchase)
        {
            return new PriceProduct
            {
                PrimarySku = product.PrimaryProductSku,
                ProductVariantSku = product.Sku,
                FabricsGroup = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                ExtraPacking = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys).ToYesOrNo(),
                SleepAreaSize = _productsService.GetSleepAreaSize(product.ProductAttributes),
                PaletteSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                Size = _productsService.GetSize(product.ProductAttributes),
                PointsOfLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                LampshadeType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                LampshadeSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                LinearLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                Mirror = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                Shape = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                BodyColour = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleBodyColorAttributeKeys)),
                ShelfType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys),
                NumberOfMirrors = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleNumberOfMirrorsAttributeKeys),
                Led = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLedAttributeKeys).ToYesOrNo(),
                IsOutlet = isOutletPurchase.ToYesOrNo()
            };
        }

        public async Task<IEnumerable<PriceProduct>> CreateAsync(IEnumerable<Product> products, bool isOutletPurchase)
        {
            var priceProducts = products.OrEmptyIfNull().Select(x => CreateAsync(x, isOutletPurchase));

            return await Task.WhenAll(priceProducts);
        }
    }
}
