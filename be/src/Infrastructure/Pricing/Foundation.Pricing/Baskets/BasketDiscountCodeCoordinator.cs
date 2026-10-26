using Foundation.Pricing.Services;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Baskets
{
    /// <summary>
    /// Shared body of the discount-code prologue that begins every basket save: reconcile the
    /// incoming request against the persisted code via <see cref="DiscountCodeResolver"/>, then
    /// accept it only if it is a Valid Discount Code for the Client. The caller owns everything
    /// that is app-specific - how to read the basket (including whether it can be read at all),
    /// which Client the basket is for and the localised messages.
    /// </summary>
    public static class BasketDiscountCodeCoordinator
    {
        public static async Task<BasketDiscountCodeOutcome> ResolveAsync(
            bool isGrulaConfigured,
            bool hasDiscountCode,
            string requestedDiscountCode,
            bool hasItems,
            Func<Task<string>> readPersistedDiscountCode,
            IDiscountCodeEligibilityRepository eligibilityRepository,
            string token,
            string language,
            Guid? clientId,
            Func<string> appliedToEmptyBasketMessage,
            Func<string> invalidDiscountCodeMessage,
            Func<string> discountCodeRemovedMessage)
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
                return BasketDiscountCodeOutcome.Rejected(appliedToEmptyBasketMessage());
            }

            if (resolution.DiscountCode is null)
            {
                // Removal never needs a check.
                return BasketDiscountCodeOutcome.ForDiscountCode(null);
            }

            var validDiscountCode = await eligibilityRepository.GetValidDiscountCodeAsync(token, language, clientId, resolution.DiscountCode);

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
                    return BasketDiscountCodeOutcome.Rejected(invalidDiscountCodeMessage());
                }
            }

            return BasketDiscountCodeOutcome.Dropped(discountCodeRemovedMessage());
        }
    }
}
