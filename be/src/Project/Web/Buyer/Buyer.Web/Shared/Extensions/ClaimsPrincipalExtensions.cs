using Buyer.Web.Shared.Definitions.Middlewares;
using System;
using System.Security.Claims;

namespace Buyer.Web.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetClientId(this ClaimsPrincipal user)
        {
            var claimValue = user?.FindFirst(ClaimsEnrichmentConstants.ClientIdClaimType)?.Value;

            return Guid.TryParse(claimValue, out var clientId)
                ? clientId
                : null;
        }
    }
}