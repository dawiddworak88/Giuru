using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The fresh product pricing seam. A discount code supplied for a new price calculation is only used when it
    /// can be newly applied for the client being priced; an inapplicable or unverifiable code is left out of the
    /// price query rather than failing it. Baskets that already store a code do not come through here - they
    /// reprice through <see cref="IBasketPriceClientResolver"/>.
    /// </summary>
    public sealed class VerifiedDiscountCodePriceClientResolver : IPriceClientResolver
    {
        private readonly IPriceClientResolver _inner;
        private readonly IDiscountCodeValidator _validator;
        private readonly IPricingSettings _pricingSettings;

        public VerifiedDiscountCodePriceClientResolver(
            IPriceClientResolver inner,
            IDiscountCodeValidator validator,
            IPricingSettings pricingSettings)
        {
            _inner = inner;
            _validator = validator;
            _pricingSettings = pricingSettings;
        }

        public async Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
        {
            // The inner resolver never sees the code: the verdict below decides whether it is used at all.
            var priceClient = await _inner.ResolveAsync(clientId, null, token);

            if (priceClient is null)
            {
                return null;
            }

            priceClient.DiscountCode = null;

            if (!_pricingSettings.IsGrulaConfigured || string.IsNullOrWhiteSpace(discountCode))
            {
                return priceClient;
            }

            // The resolved client is the one being priced, which is not necessarily the id the caller passed in:
            // the buyer resolver takes it from the signed-in principal and rejects an explicit one.
            var validation = await _validator.ValidateAsync(priceClient.Id, discountCode, token);

            if (validation.IsValid)
            {
                priceClient.DiscountCode = validation.DiscountCode;
            }

            return priceClient;
        }
    }
}
