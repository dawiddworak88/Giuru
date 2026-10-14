using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Pricing.Baskets;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// Proof that a code was validated for the client whose basket was priced. Basket IDs alone are not proof of
    /// ownership: their cookies survive logout and Basket.Api does not bind a basket to a client. Data protection
    /// binds this proof to both identities, using the buyer app's existing shared key ring.
    /// </summary>
    public sealed class BasketDiscountCodeBinding : IBasketDiscountCodeBinding
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public BasketDiscountCodeBinding(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public bool IsBoundToClient(Guid? basketId, Guid? clientId, string discountCode)
        {
            if (!HasIdentity(basketId, clientId) || string.IsNullOrWhiteSpace(discountCode))
            {
                return false;
            }

            var proof = _httpContextAccessor.HttpContext?.Request.Cookies[BasketConstants.DiscountCodeBindingCookieName];

            if (string.IsNullOrEmpty(proof))
            {
                return false;
            }

            try
            {
                var boundCode = CreateProtector(basketId.Value, clientId.Value).Unprotect(proof);

                return DiscountCodeResolver.IsStoredDiscountCode(discountCode, boundCode);
            }
            catch (CryptographicException)
            {
                // Missing, tampered or foreign proofs grant no exemption from validation.
                return false;
            }
        }

        public void Bind(Guid? basketId, Guid? clientId, string discountCode)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
            {
                return;
            }

            if (!HasIdentity(basketId, clientId) || string.IsNullOrWhiteSpace(discountCode))
            {
                context.Response.Cookies.Delete(BasketConstants.DiscountCodeBindingCookieName);
                return;
            }

            var proof = CreateProtector(basketId.Value, clientId.Value).Protect(discountCode.Trim());

            context.Response.Cookies.Append(BasketConstants.DiscountCodeBindingCookieName, proof, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                MaxAge = TimeSpan.FromDays(BasketConstants.BasketCookieMaxAge)
            });
        }

        private IDataProtector CreateProtector(Guid basketId, Guid clientId)
        {
            return _dataProtectionProvider.CreateProtector("Buyer.Web.BasketDiscountCodeBinding.v1", basketId.ToString("N"), clientId.ToString("N"));
        }

        private static bool HasIdentity(Guid? basketId, Guid? clientId)
        {
            return basketId.HasValue && basketId.Value != Guid.Empty && clientId.HasValue && clientId.Value != Guid.Empty;
        }
    }
}
