using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Extensions;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    /// <summary>
    /// Buyers only ever price for themselves, so the client is taken from the authenticated
    /// principal. <paramref name="token"/> is only used to check the discount code with Client.Api.
    /// </summary>
    public class ClaimsPriceClientResolver : IPriceClientResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicableDiscountCodeService _applicableDiscountCodeService;

        public ClaimsPriceClientResolver(
            IHttpContextAccessor httpContextAccessor,
            IApplicableDiscountCodeService applicableDiscountCodeService)
        {
            _httpContextAccessor = httpContextAccessor;
            _applicableDiscountCodeService = applicableDiscountCodeService;
        }

        public async Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
        {
            if (clientId.HasValue)
            {
                // Silently ignoring it would price the signed-in buyer's basket against somebody
                // else's identity, so fail loudly instead.
                throw new ArgumentException(
                    "The buyer app prices for the authenticated principal and cannot price for an explicit client id.",
                    nameof(clientId));
            }

            var user = _httpContextAccessor.HttpContext?.User;

            // An invalid code is priced as no code at all - a price lookup never fails on it.
            var validDiscountCode = await _applicableDiscountCodeService.GetValidAsync(token, discountCode);

            return new PriceClient
            {
                Id = user.GetClientId(),
                Name = user?.FindFirst(ClaimsEnrichmentConstants.ClientNameClaimType)?.Value,
                CurrencyCode = user?.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                ExtraPacking = user?.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                PaletteLoading = user?.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                Country = user?.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                DeliveryZipCode = user?.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value,
                DiscountCode = validDiscountCode
            };
        }
    }
}
