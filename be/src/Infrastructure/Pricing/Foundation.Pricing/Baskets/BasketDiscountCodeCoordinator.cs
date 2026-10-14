using Foundation.Pricing.DiscountCodes;
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
    /// This is also the basket write seam of discount code enforcement. A newly applied code that is not
    /// applicable is rejected, and one whose applicability cannot be determined is rejected too, so the
    /// basket is never priced with an unverified code. A code that is already stored on the basket is
    /// treated according to <c>revalidateStoredDiscountCode</c>:
    /// <list type="bullet">
    /// <item>A buyer basket with proof that its code was validated for the current client keeps that code
    /// through edits and uploads, even if it has since been revoked. A missing or mismatched binding
    /// requires validation again.</item>
    /// <item>A basket that names its client on every save (the seller's) is not bound to any client, so a
    /// stored code is validated for the client of every save that prices it, exactly like a new code.
    /// Otherwise a code applied for one client would carry its discount into another client's basket.</item>
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
            bool revalidateStoredDiscountCode)
        {
            // The persisted code is read at most once however many of the steps below need it.
            var persistedDiscountCode = new Lazy<Task<string>>(readPersistedDiscountCode);

            if (!isGrulaConfigured)
            {
                // Discount codes are Grula price drivers. While pricing is unavailable,
                // ignore incoming mutations but retain any code already stored on the basket.
                return BasketDiscountCodeOutcome.ForDiscountCode(await persistedDiscountCode.Value);
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

            // Nor does a code that is already stored, unless the caller revalidates stored codes - and even then only
            // when this save prices it: an emptied basket keeps its code unpriced until lines are added again.
            var isStoredDiscountCode = !isNewOrReplacement || isUnchanged;

            if (isStoredDiscountCode && (!revalidateStoredDiscountCode || !hasItems))
            {
                return BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
            }

            var validation = await validateDiscountCode(resolution.DiscountCode);

            return validation.IsValid
                ? BasketDiscountCodeOutcome.ForDiscountCode(validation.DiscountCode)
                : BasketDiscountCodeOutcome.Rejected(rejectionMessage(validation.Status));
        }
    }
}
