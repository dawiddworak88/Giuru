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

        /// <summary>A save dropped the stored code, which is no longer valid, and priced the basket without it.</summary>
        public static string GetRemovalMessage(IStringLocalizer<OrderResources> orderLocalizer, string removedDiscountCode)
        {
            return orderLocalizer.GetString("DiscountCodeRemovedPricesUpdated", removedDiscountCode).Value;
        }

        /// <summary>
        /// Checkout found the stored code no longer valid. The order would keep the prices it was saved with, so the buyer
        /// is told to remove the code - which reprices the basket - rather than why it cannot be applied.
        /// </summary>
        public static string GetCheckoutRejectionMessage(IStringLocalizer<OrderResources> orderLocalizer, string discountCode)
        {
            return orderLocalizer.GetString("DiscountCodeNoLongerValid", discountCode).Value;
        }
    }
}
