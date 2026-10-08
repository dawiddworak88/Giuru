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
                DiscountCodeValidationStatus.Unavailable => orderLocalizer.GetString("DiscountCodeCouldNotBeVerified").Value,
                _ => orderLocalizer.GetString("DiscountCodeInvalid").Value
            };
        }

        public static string GetRemovedMessage(IStringLocalizer<OrderResources> orderLocalizer, string removedDiscountCode)
        {
            return orderLocalizer.GetString("DiscountCodeRemovedFromBasket", removedDiscountCode).Value;
        }
    }
}
