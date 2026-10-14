using Buyer.Web.Shared.Definitions.Basket;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class BasketDiscountCodeBindingTests
    {
        [Fact]
        public async Task AValidatedBasketGetsItsSecurityCookieWithoutOptionalCookieConsent()
        {
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            var middleware = new CookiePolicyMiddleware(request =>
            {
                DiscountCodeBindingTestHelper.Create(request).Bind(basketId, clientId, "STORED25");
                return Task.CompletedTask;
            }, Options.Create(new CookiePolicyOptions { CheckConsentNeeded = _ => true }), NullLoggerFactory.Instance);

            await middleware.Invoke(context);

            var cookie = Assert.Single(SetCookieHeaderValue.ParseList(context.Response.Headers.SetCookie.ToArray()));
            Assert.Equal(BasketConstants.DiscountCodeBindingCookieName, cookie.Name.ToString());
            Assert.True(cookie.HttpOnly);
            Assert.True(cookie.Secure);
            Assert.Equal(TimeSpan.FromDays(BasketConstants.BasketCookieMaxAge), cookie.MaxAge);
            Assert.True(DiscountCodeBindingTestHelper.HasNewBinding(context, basketId, clientId, "STORED25"));
        }
    }
}
