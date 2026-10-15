using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Baskets
{
    /// <summary>
    /// Shared body of the discount-code prologue that begins every basket save: reconcile the
    /// incoming request against the persisted code via <see cref="DiscountCodeResolver"/>, reading
    /// the basket back only when it is needed. The caller owns everything that is app-specific - how
    /// to read the basket (including whether it can be read at all), how to validate a code for the
    /// client, and the localised messages to use if the request must be rejected.
    ///
    /// This is also the basket write seam of discount code enforcement, with one rule for every app: every code a save
    /// prices is validated for the client of that save.
    /// <list type="bullet">
    /// <item>A newly applied code is stored only once it is known to be applicable. One that is not, or whose
    /// applicability cannot be determined, is rejected, and the basket keeps what it had.</item>
    /// <item>A code already stored on the basket that is no longer applicable - revoked since it was stored, or the
    /// basket is now saved for another client - is removed: the save goes ahead priced without it, and the outcome
    /// names the removed code so that the caller can tell the user why the prices changed.</item>
    /// <item>A stored code whose applicability cannot be determined right now stays in force, and a warning is logged,
    /// so an outage of the lookup neither blocks the save nor changes the basket's prices.</item>
    /// </list>
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
            Func<string, Task<DiscountCodeValidation>> validateDiscountCode,
            Func<DiscountCodeValidationStatus, string> rejectionMessage,
            ILogger logger)
        {
            // The persisted code is read at most once however many of the steps below need it.
            var persistedDiscountCode = new Lazy<Task<string>>(readPersistedDiscountCode);

            if (!isGrulaConfigured)
            {
                // Discount codes are Grula price drivers. While pricing is unavailable no code is applied, but any code
                // already stored on the basket is retained - unless the request removes it, which needs no pricing.
                return BasketDiscountCodeOutcome.ForDiscountCode(hasDiscountCode && requestedDiscountCode is null
                    ? null
                    : await persistedDiscountCode.Value);
            }

            var isNewOrReplacement = hasDiscountCode && !string.IsNullOrWhiteSpace(requestedDiscountCode);
            var existingDiscountCode = isNewOrReplacement || DiscountCodeResolver.RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems)
                ? await persistedDiscountCode.Value
                : null;

            // The same text as the stored code is the stored code, even when the frontend sends it again with
            // other letter casing or spacing. Handing the resolver the stored spelling keeps its rule that
            // emptying a discounted basket is not "applying a code to an empty basket".
            var isUnchanged = isNewOrReplacement
                && DiscountCodeResolver.IsStoredDiscountCode(requestedDiscountCode, existingDiscountCode);
            var resolution = DiscountCodeResolver.Resolve(
                hasDiscountCode,
                isUnchanged ? existingDiscountCode.Trim() : requestedDiscountCode,
                existingDiscountCode,
                hasItems);

            if (resolution.IsAppliedToEmptyBasket)
            {
                return BasketDiscountCodeOutcome.Rejected(appliedToEmptyBasketMessage());
            }

            // A removal and a basket without a code need no validation.
            if (string.IsNullOrWhiteSpace(resolution.DiscountCode))
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
            }

            // Nor does a stored code that this save does not price: an emptied basket keeps its code unpriced, and the
            // code is validated again as soon as lines are added.
            var isStoredDiscountCode = !isNewOrReplacement || isUnchanged;

            if (isStoredDiscountCode && !hasItems)
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
            }

            var validation = await validateDiscountCode(resolution.DiscountCode);

            if (validation.IsValid)
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(validation.DiscountCode);
            }

            if (!isStoredDiscountCode)
            {
                return BasketDiscountCodeOutcome.Rejected(rejectionMessage(validation.Status));
            }

            if (validation.Status is DiscountCodeValidationStatus.Unavailable)
            {
                logger.LogWarning("The discount code stored on the basket could not be verified. It is kept, and the basket is priced with it.");

                return BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
            }

            return BasketDiscountCodeOutcome.Removed(resolution.DiscountCode);
        }
    }
}
