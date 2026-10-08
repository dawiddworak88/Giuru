using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Localization;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// A buyer is never told why a code is not applicable - a code that does not exist, one that is disabled and one that
    /// is assigned to someone else all read the same - so that codes cannot be probed.
    /// </summary>
    public static class DiscountCodeMessages
    {
        public static string GetRejectionMessage(IStringLocalizer<OrderResources> orderLocalizer, DiscountCodeValidationStatus status)
        {
            return status is DiscountCodeValidationStatus.Unavailable
                ? orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value
                : orderLocalizer.GetString("DiscountCodeInvalid").Value;
        }

        public static string GetRemovedMessage(IStringLocalizer<OrderResources> orderLocalizer, string removedDiscountCode)
        {
            return orderLocalizer.GetString("DiscountCodeRemovedFromBasket", removedDiscountCode).Value;
        }
    }
}
