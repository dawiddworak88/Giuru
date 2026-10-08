using Foundation.Pricing.Configurations;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The pricing seam of discount code enforcement. It wraps a concrete <see cref="IPriceClientResolver"/> and lets a
    /// code through to Grula only when it exists and can be applied for that client, replacing it with the canonical
    /// spelling - or with <c>null</c> when it is not applicable or could not be verified. Every pricing path that accepts
    /// a code (basket writes, product prices, catalog and SSR builders) goes through the resolver, so this one place
    /// covers them all. <see cref="IPriceService"/> itself takes a ready <see cref="PriceClient"/> and is not a boundary:
    /// a future pricing path must also resolve its client through this decorator.
    /// </summary>
    public class VerifiedDiscountCodePriceClientResolver : IPriceClientResolver
    {
        private readonly IPriceClientResolver _inner;
        private readonly IDiscountCodeValidator _validator;
        private readonly IPricingSettings _settings;

        public VerifiedDiscountCodePriceClientResolver(
            IPriceClientResolver inner,
            IDiscountCodeValidator validator,
            IPricingSettings settings)
        {
            _inner = inner;
            _validator = validator;
            _settings = settings;
        }

        public async Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
        {
            var enforce = _settings.IsDiscountCodeEnforced();

            // With enforcement on the inner resolver never sees the unverified code.
            var priceClient = await _inner.ResolveAsync(clientId, enforce ? null : discountCode, token);

            if (!enforce || priceClient is null)
            {
                return priceClient;
            }

            if (string.IsNullOrWhiteSpace(discountCode))
            {
                priceClient.DiscountCode = null;

                return priceClient;
            }

            // The inner client's id, not the argument: a principal-based resolver takes the client from the claims
            // and rejects an explicit id, so the argument may be null.
            var validation = await _validator.ValidateAsync(priceClient.Id, discountCode, token);

            priceClient.DiscountCode = validation.IsValid ? validation.DiscountCode : null;

            return priceClient;
        }
    }
}
