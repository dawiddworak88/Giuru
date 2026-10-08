using Foundation.Pricing.DiscountCodes;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Baskets
{
    /// <summary>
    /// Shared body of the discount-code prologue that begins every basket save: reconcile the
    /// incoming request against the persisted code via <see cref="DiscountCodeResolver"/>, reading
    /// the basket back only when the resolver says it is needed. The caller owns everything that is
    /// app-specific - how to read the basket (including whether it can be read at all) and the
    /// localised message to use if the request must be rejected.
    ///
    /// This is also the basket write seam of discount code enforcement, the one that gives the user feedback. With
    /// enforcement on, a newly applied code that is not applicable is rejected, a stored code that has become
    /// inapplicable is dropped and reported, and a code whose applicability cannot be determined is rejected - it fails
    /// closed rather than pricing with an unverified code or silently raising a customer's prices on a transient error.
    /// </summary>
    public static class BasketDiscountCodeCoordinator
    {
        public static async Task<BasketDiscountCodeOutcome> ResolveAsync(
            bool isGrulaConfigured,
            bool hasDiscountCode,
            string requestedDiscountCode,
            bool hasItems,
            Func<Task<string>> readPersistedDiscountCode,
            Func<string> appliedToEmptyBasketMessage,
            bool isDiscountCodeEnforced = false,
            Func<string, Task<DiscountCodeValidation>> validateDiscountCode = null,
            Func<DiscountCodeValidationStatus, string> rejectionMessage = null)
        {
            // The persisted code is read at most once however many of the steps below need it.
            var persistedDiscountCode = new Lazy<Task<string>>(readPersistedDiscountCode);

            if (!isGrulaConfigured)
            {
                // Discount codes are Grula price drivers. While pricing is unavailable,
                // ignore incoming mutations but retain any code already stored on the basket.
                return BasketDiscountCodeOutcome.ForDiscountCode(await persistedDiscountCode.Value);
            }

            var existingDiscountCode = DiscountCodeResolver.RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems)
                ? await persistedDiscountCode.Value
                : null;
            var resolution = DiscountCodeResolver.Resolve(hasDiscountCode, requestedDiscountCode, existingDiscountCode, hasItems);

            if (resolution.IsAppliedToEmptyBasket)
            {
                return BasketDiscountCodeOutcome.Rejected(appliedToEmptyBasketMessage());
            }

            // A removal needs no validation, and neither does a basket without a code.
            if (!isDiscountCodeEnforced || string.IsNullOrWhiteSpace(resolution.DiscountCode))
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
            }

            if (validateDiscountCode is null || rejectionMessage is null)
            {
                throw new InvalidOperationException("Discount code enforcement needs a validation function and a rejection message factory.");
            }

            var validation = await validateDiscountCode(resolution.DiscountCode);

            if (validation.IsValid)
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(validation.DiscountCode);
            }

            if (validation.Status is DiscountCodeValidationStatus.Unavailable)
            {
                return BasketDiscountCodeOutcome.Rejected(rejectionMessage(validation.Status));
            }

            // Not applicable. A code that is already on the basket is dropped, because rejecting the save would leave
            // the user unable to change a basket that can no longer be priced with it; a code that is new is rejected.
            var storedDiscountCode = (await persistedDiscountCode.Value)?.Trim();

            return string.Equals(storedDiscountCode, resolution.DiscountCode.Trim(), StringComparison.OrdinalIgnoreCase)
                ? BasketDiscountCodeOutcome.Removed(storedDiscountCode)
                : BasketDiscountCodeOutcome.Rejected(rejectionMessage(validation.Status));
        }
    }
}
