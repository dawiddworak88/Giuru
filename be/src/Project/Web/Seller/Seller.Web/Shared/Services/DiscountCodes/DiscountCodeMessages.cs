using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Localization;

namespace Seller.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// Sellers can fix what is wrong, so unlike buyers they are told the specific reason a code cannot be applied.
    /// </summary>
    public static class DiscountCodeMessages
    {
        public static string GetRejectionMessage(IStringLocalizer<OrderResources> orderLocalizer, DiscountCodeValidationStatus status)
        {
            return status switch
            {
                DiscountCodeValidationStatus.Disabled => orderLocalizer.GetString("DiscountCodeDisabled").Value,
                DiscountCodeValidationStatus.NotAssigned => orderLocalizer.GetString("DiscountCodeNotAssignedToClient").Value,
                DiscountCodeValidationStatus.ClientUnknown => orderLocalizer.GetString("DiscountCodeClientRequired").Value,
                DiscountCodeValidationStatus.Unavailable => orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value,
                _ => orderLocalizer.GetString("DiscountCodeInvalid").Value
            };
        }

        /// <summary>A save dropped the stored code, which is no longer valid, and priced the basket without it.</summary>
        public static string GetRemovalMessage(IStringLocalizer<OrderResources> orderLocalizer, string removedDiscountCode)
        {
            return orderLocalizer.GetString("DiscountCodeRemovedPricesUpdated", removedDiscountCode).Value;
        }
    }
}
