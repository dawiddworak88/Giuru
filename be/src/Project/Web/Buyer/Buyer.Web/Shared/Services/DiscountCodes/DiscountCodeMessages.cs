using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// Buyers are not told why a code cannot be applied, only that it cannot, or that it could not be checked right now.
    /// </summary>
    public static class DiscountCodeMessages
    {
        public static string GetRejectionMessage(IStringLocalizer<OrderResources> orderLocalizer, DiscountCodeValidationStatus status)
        {
            return status is DiscountCodeValidationStatus.Unavailable
                ? orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value
                : orderLocalizer.GetString("DiscountCodeInvalid").Value;
        }
    }
}
