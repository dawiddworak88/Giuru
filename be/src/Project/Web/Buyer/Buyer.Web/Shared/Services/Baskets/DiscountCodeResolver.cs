using System;

namespace Buyer.Web.Shared.Services.Baskets
{
    /// <summary>
    /// Single source of truth for how an incoming discount code is reconciled with the
    /// one already persisted on the basket. Every entry point that writes a basket
    /// (JSON body or multipart upload) must go through here so that the same request
    /// always produces the same code.
    /// </summary>
    public static class DiscountCodeResolver
    {
        public static string ResolveDiscountCode(bool hasDiscountCode, string requestedDiscountCode, string persistedDiscountCode)
        {
            if (!hasDiscountCode)
            {
                return Normalize(persistedDiscountCode);
            }

            if (requestedDiscountCode is null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(requestedDiscountCode))
            {
                return Normalize(persistedDiscountCode);
            }

            return requestedDiscountCode.Trim();
        }

        public static DiscountCodeResolution Resolve(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems)
        {
            var persistedCode = Normalize(persistedDiscountCode);
            var discountCode = ResolveDiscountCode(hasDiscountCode, requestedDiscountCode, persistedDiscountCode);

            // Same order as ResolveDiscountCode: an explicit null has to be recognised as a
            // removal before the blank check, which would otherwise swallow it.
            if (!hasDiscountCode)
            {
                // The request has no opinion on the code, so the persisted basket stays
                // authoritative. Still reprice while a code is active, otherwise a stale
                // client could add undiscounted lines to a discounted basket.
                return new DiscountCodeResolution(discountCode, persistedCode is not null, false);
            }

            if (requestedDiscountCode is null)
            {
                // Explicit removal. Reprice unconditionally: the persisted code was not
                // read back, so the discounted prices have to be recalculated regardless.
                return new DiscountCodeResolution(null, true, false);
            }

            if (string.IsNullOrWhiteSpace(requestedDiscountCode))
            {
                return new DiscountCodeResolution(discountCode, persistedCode is not null, false);
            }

            return new DiscountCodeResolution(
                discountCode,
                true,
                !hasItems && !string.Equals(discountCode, persistedCode, StringComparison.Ordinal));
        }

        /// <summary>
        /// Whether the caller has to read the basket back before resolving. An explicit
        /// code or an explicit removal is self-contained; an omitted or blank code means
        /// "unchanged" and can only be resolved against what is stored.
        /// </summary>
        public static bool RequiresPersistedDiscountCode(bool hasDiscountCode, string requestedDiscountCode, bool hasItems)
        {
            if (!hasDiscountCode)
            {
                return true;
            }

            if (requestedDiscountCode is null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(requestedDiscountCode))
            {
                return true;
            }

            // An explicit code resolves on its own. The persisted code is still needed to
            // tell "applying a code to an empty basket" from "emptying a discounted one".
            return !hasItems;
        }

        private static string Normalize(string discountCode)
        {
            return string.IsNullOrWhiteSpace(discountCode) ? null : discountCode.Trim();
        }
    }
}
