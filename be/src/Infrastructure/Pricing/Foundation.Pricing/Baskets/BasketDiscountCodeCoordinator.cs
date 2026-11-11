using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Baskets
{
    /// <summary>
    /// Shared body of the discount-code prologue that begins every basket save: reconcile the
    /// incoming request against the persisted code via <see cref="DiscountCodeResolver"/>, then
    /// accept it only if it is a Valid Discount Code for the Client. The caller owns everything
    /// that is app-specific - how to read the basket (including whether it can be read at all)
    /// and which Client the eligibility check is made for.
    /// </summary>
    public static class BasketDiscountCodeCoordinator
    {
        /// <param name="getValidDiscountCode">
        /// The eligibility check, already bound to the basket's Client: returns the code's stored
        /// spelling when it is valid for that Client, otherwise null.
        /// </param>
        /// <param name="orderLocalizer">Localizer over the order resources that hold the discount-code messages.</param>
        public static async Task<BasketDiscountCodeOutcome> ResolveAsync(
            bool isGrulaConfigured,
            bool hasDiscountCode,
            string requestedDiscountCode,
            bool hasItems,
            Func<Task<string>> readPersistedDiscountCode,
            Func<string, Task<string>> getValidDiscountCode,
            IStringLocalizer orderLocalizer)
        {
            if (!isGrulaConfigured)
            {
                // Discount codes are Grula price drivers. While pricing is unavailable,
                // ignore incoming mutations but retain any code already stored on the basket.
                var persistedDiscountCode = await readPersistedDiscountCode();

                return BasketDiscountCodeOutcome.ForDiscountCode(persistedDiscountCode);
            }

            var existingDiscountCode = DiscountCodeResolver.RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems)
                ? await readPersistedDiscountCode()
                : null;
            var resolution = DiscountCodeResolver.Resolve(hasDiscountCode, requestedDiscountCode, existingDiscountCode, hasItems);

            if (resolution.IsAppliedToEmptyBasket)
            {
                return BasketDiscountCodeOutcome.Rejected(orderLocalizer.GetString("DiscountCodeRequiresBasketItems").Value);
            }

            if (resolution.DiscountCode is null)
            {
                // Removal never needs a check.
                return BasketDiscountCodeOutcome.ForDiscountCode(null);
            }

            var validDiscountCode = await getValidDiscountCode(resolution.DiscountCode);

            if (validDiscountCode is not null)
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(validDiscountCode);
            }

            var isNewCode = hasDiscountCode && !string.IsNullOrWhiteSpace(requestedDiscountCode);

            if (isNewCode)
            {
                // A client that re-sends the code already on the basket is not applying a new one.
                var storedDiscountCode = existingDiscountCode ?? await readPersistedDiscountCode();

                if (!string.Equals(storedDiscountCode?.Trim(), resolution.DiscountCode, StringComparison.OrdinalIgnoreCase))
                {
                    return BasketDiscountCodeOutcome.Rejected(orderLocalizer.GetString("DiscountCodeInvalid").Value);
                }
            }

            return BasketDiscountCodeOutcome.Dropped(orderLocalizer.GetString("DiscountCodeRemoved").Value);
        }
    }
}
