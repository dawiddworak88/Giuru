using Foundation.Pricing.Baskets;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The fresh product pricing seam. It applies the rule of the basket write seam
    /// (<see cref="BasketDiscountCodeCoordinator"/>), so a code is not priced one way on a product page and another in
    /// the basket: a code - including the one stored on the basket - is only used when it is applicable for the client
    /// being priced right now. An inapplicable or unverifiable code is left out of the price query rather than failing
    /// it. The one difference is an outage of the lookup: a basket save keeps pricing with the code already stored on
    /// the basket, while a price read, which cannot tell that code from any other, shows prices without it until the
    /// code can be verified again. Basket saves do not come through here - they reprice through
    /// <see cref="IBasketPriceClientResolver"/>.
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
