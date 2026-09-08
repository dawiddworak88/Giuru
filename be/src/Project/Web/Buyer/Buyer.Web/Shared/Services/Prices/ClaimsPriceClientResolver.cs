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
    /// principal. <paramref name="token"/> is unused here for the same reason - nothing is
    /// looked up over the API.
    /// </summary>
    public class ClaimsPriceClientResolver : IPriceClientResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPriceClientResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
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

            return Task.FromResult(new PriceClient
            {
                Id = user.GetClientId(),
                Name = user?.Identity?.Name,
                CurrencyCode = user?.FindFirst(ClaimsEnrichmentConstants.CurrencyClaimType)?.Value,
                ExtraPacking = user?.FindFirst(ClaimsEnrichmentConstants.ExtraPackingClaimType)?.Value,
                PaletteLoading = user?.FindFirst(ClaimsEnrichmentConstants.PaletteLoadingClaimType)?.Value,
                Country = user?.FindFirst(ClaimsEnrichmentConstants.CountryClaimType)?.Value,
                DeliveryZipCode = user?.FindFirst(ClaimsEnrichmentConstants.ZipCodeClaimType)?.Value,
                DiscountCode = discountCode
            });
        }
    }
}
