using Foundation.Extensions.ExtensionMethods;

namespace Buyer.Web.Shared.Services.Prices
{
    internal static class PriceDriverValueNormalizer
    {
        internal static string NormalizeExtraPacking(string value)
        {
            return value.ToYesOrNo();
        }
    }
}
