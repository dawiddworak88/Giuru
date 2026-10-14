using Foundation.Pricing.Baskets;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The fresh product pricing seam. It applies the same rule as the basket write seam
    /// (<see cref="BasketDiscountCodeCoordinator"/>), so a code is never priced one way on a product page and another
    /// in the basket: a code the app keeps stored on the basket without revalidation (see
    /// <see cref="IStoredDiscountCodeReader"/>) is in force as it is, even if it has since been revoked, and any other
    /// code is only used when it can be newly applied for the client being priced. An inapplicable or unverifiable code
    /// is left out of the price query rather than failing it. Basket saves do not come through here - they reprice
    /// through <see cref="IBasketPriceClientResolver"/>.
    /// </summary>
    public sealed class VerifiedDiscountCodePriceClientResolver : IPriceClientResolver
    {
        private readonly IPriceClientResolver _inner;
        private readonly IDiscountCodeValidator _validator;
        private readonly IStoredDiscountCodeReader _storedDiscountCodeReader;
        private readonly IPricingSettings _pricingSettings;
        private readonly ILogger<VerifiedDiscountCodePriceClientResolver> _logger;

        public VerifiedDiscountCodePriceClientResolver(
            IPriceClientResolver inner,
            IDiscountCodeValidator validator,
            IStoredDiscountCodeReader storedDiscountCodeReader,
            IPricingSettings pricingSettings,
            ILogger<VerifiedDiscountCodePriceClientResolver> logger)
        {
            _inner = inner;
            _validator = validator;
            _storedDiscountCodeReader = storedDiscountCodeReader;
            _pricingSettings = pricingSettings;
            _logger = logger;
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

            var storedDiscountCode = await ReadStoredDiscountCodeAsync(priceClient.Id, token);

            if (DiscountCodeResolver.IsStoredDiscountCode(discountCode, storedDiscountCode))
            {
                // Priced with the stored spelling, as the basket reprices it.
                priceClient.DiscountCode = storedDiscountCode.Trim();

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

        private async Task<string> ReadStoredDiscountCodeAsync(Guid? clientId, string token)
        {
            try
            {
                return await _storedDiscountCodeReader.ReadAsync(clientId, token);
            }
            catch (Exception ex)
            {
                // Without the stored code nothing is known to be in force, so the code has to pass validation instead.
                _logger.LogWarning(ex, "The discount code stored on the basket could not be read. The requested code is validated instead.");

                return null;
            }
        }
    }
}
