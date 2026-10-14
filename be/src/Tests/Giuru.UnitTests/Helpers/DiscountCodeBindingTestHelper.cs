using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Services.DiscountCodes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NSubstitute;
using System;
using System.Linq;

namespace Giuru.UnitTests.Helpers
{
    internal static class DiscountCodeBindingTestHelper
    {
        private static readonly IDataProtectionProvider Provider = new EphemeralDataProtectionProvider();

        public static BasketDiscountCodeBinding Create(HttpContext context)
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            accessor.HttpContext.Returns(context);

            return new BasketDiscountCodeBinding(accessor, Provider);
        }

        public static void AddProof(HttpContext request, Guid basketId, Guid? clientId, string code)
        {
            var previousRequest = new DefaultHttpContext();
            Create(previousRequest).Bind(basketId, clientId, code);
            CopyProof(previousRequest, request);
        }

        public static void CopyProof(HttpContext response, HttpContext request)
        {
            var cookie = SetCookieHeaderValue.ParseList(response.Response.Headers.SetCookie.ToArray())
                .Single(x => x.Name == BasketConstants.DiscountCodeBindingCookieName);
            var existingCookies = request.Request.Headers.Cookie.ToString();
            request.Request.Headers.Cookie = string.IsNullOrEmpty(existingCookies)
                ? $"{cookie.Name}={cookie.Value}"
                : $"{existingCookies}; {cookie.Name}={cookie.Value}";
        }

        public static bool HasNewBinding(HttpContext response, Guid basketId, Guid? clientId, string code)
        {
            if (!response.Response.Headers.SetCookie.Any(x => x.StartsWith(BasketConstants.DiscountCodeBindingCookieName + "=", StringComparison.Ordinal)))
            {
                return false;
            }

            var nextRequest = new DefaultHttpContext();
            CopyProof(response, nextRequest);

            return Create(nextRequest).IsBoundToClient(basketId, clientId, code);
        }
    }
}
