using Foundation.Extensions.ExtensionMethods;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public static class PriceProductBuilder
    {
        public static async Task<PriceProduct> BuildAsync(
            string primarySku,
            string productVariantSku,
            IProductAttributeReader attributes,
            IProductColorTranslator colors,
            IPriceProductAttributeKeys keys,
            bool isOutletPurchase)
        {
            return new PriceProduct
            {
                PrimarySku = primarySku,
                ProductVariantSku = productVariantSku,
                FabricsGroup = attributes.GetFirstAvailableAttributeValue(keys.PossiblePriceGroupAttributeKeys),
                ExtraPacking = attributes.GetFirstAvailableAttributeValue(keys.PossibleExtraPackingAttributeKeys).ToYesOrNo(),
                SleepAreaSize = attributes.GetSleepAreaSize(),
                PaletteSize = attributes.GetFirstAvailableAttributeValue(keys.PossiblePaletteSizeAttributeKeys),
                Size = attributes.GetSize(),
                PointsOfLight = attributes.GetFirstAvailableAttributeValue(keys.PossiblePointsOfLightAttributeKeys),
                LampshadeType = attributes.GetFirstAvailableAttributeValue(keys.PossibleLampshadeTypeAttributeKeys),
                LampshadeSize = attributes.GetFirstAvailableAttributeValue(keys.PossibleLampshadeSizeAttributeKeys),
                LinearLight = attributes.GetFirstAvailableAttributeValue(keys.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                Mirror = attributes.GetFirstAvailableAttributeValue(keys.PossibleMirrorAttributeKeys).ToYesOrNo(),
                Shape = attributes.GetFirstAvailableAttributeValue(keys.PossibleShapeAttributeKeys),
                PrimaryColor = await colors.ToEnglishAsync(attributes.GetFirstAvailableAttributeValue(keys.PossiblePrimaryColorAttributeKeys)),
                SecondaryColor = await colors.ToEnglishAsync(attributes.GetFirstAvailableAttributeValue(keys.PossibleSecondaryColorAttributeKeys)),
                BodyColour = await colors.ToEnglishAsync(attributes.GetFirstAvailableAttributeValue(keys.PossibleBodyColorAttributeKeys)),
                ShelfType = attributes.GetFirstAvailableAttributeValue(keys.PossibleShelfTypeAttributeKeys),
                NumberOfMirrors = attributes.GetFirstAvailableAttributeValue(keys.PossibleNumberOfMirrorsAttributeKeys),
                Led = attributes.GetFirstAvailableAttributeValue(keys.PossibleLedAttributeKeys).ToYesOrNo(),
                IsOutlet = isOutletPurchase.ToYesOrNo()
            };
        }
    }
}
