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
    /// </summary>
    public static class BasketDiscountCodeCoordinator
    {
        public static async Task<BasketDiscountCodeOutcome> ResolveAsync(
            bool isGrulaConfigured,
            bool hasDiscountCode,
            string requestedDiscountCode,
            bool hasItems,
            Func<Task<string>> readPersistedDiscountCode,
            Func<string> appliedToEmptyBasketMessage)
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

            return resolution.IsAppliedToEmptyBasket
                ? BasketDiscountCodeOutcome.Rejected(appliedToEmptyBasketMessage())
                : BasketDiscountCodeOutcome.ForDiscountCode(resolution.DiscountCode);
        }
    }
}
