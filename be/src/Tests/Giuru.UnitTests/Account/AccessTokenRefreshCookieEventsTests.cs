using Foundation.Account.Events;
using Foundation.Account.Services.Tokens;
using Foundation.Account.ServicesModels.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Account
{
    public class AccessTokenRefreshCookieEventsTests
    {
        private const string CookiesScheme = "Cookies";
        private static readonly DateTimeOffset Now = new(2026, 11, 25, 12, 0, 0, TimeSpan.Zero);

        [Fact]
        public async Task ValidatePrincipal_AccessTokenValidForLonger_DoesNotRefresh()
        {
            var refreshService = Substitute.For<IAccessTokenRefreshService>();
            var context = CreateContext(Now.AddMinutes(30));

            await CreateEvents(refreshService).ValidatePrincipal(context);

            await refreshService.DidNotReceive().RefreshAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
            Assert.False(context.ShouldRenew);
            Assert.Equal("old-access-token", context.Properties.GetTokenValue("access_token"));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(0)]
        [InlineData(-30)]
        public async Task ValidatePrincipal_AccessTokenAboutToExpireOrExpired_StoresRefreshedTokensAndRenewsCookie(int minutesUntilExpiration)
        {
            var refreshService = CreateRefreshService(new RefreshedTokensServiceModel
            {
                Succeeded = true,
                AccessToken = "new-access-token",
                RefreshToken = "new-refresh-token",
                IdentityToken = "new-id-token",
                ExpiresIn = 3600
            });
            var context = CreateContext(Now.AddMinutes(minutesUntilExpiration));

            await CreateEvents(refreshService).ValidatePrincipal(context);

            await refreshService.Received(1).RefreshAsync("refresh-token", Arg.Any<CancellationToken>());
            Assert.True(context.ShouldRenew);
            Assert.NotNull(context.Principal);
            Assert.Equal("new-access-token", context.Properties.GetTokenValue("access_token"));
            Assert.Equal("new-refresh-token", context.Properties.GetTokenValue("refresh_token"));
            Assert.Equal("new-id-token", context.Properties.GetTokenValue("id_token"));
            Assert.Equal(Now.AddHours(1), DateTimeOffset.Parse(context.Properties.GetTokenValue("expires_at"), CultureInfo.InvariantCulture));
        }

        [Fact]
        public async Task ValidatePrincipal_RefreshWithoutNewRefreshOrIdentityToken_KeepsTheStoredOnes()
        {
            var refreshService = CreateRefreshService(new RefreshedTokensServiceModel
            {
                Succeeded = true,
                AccessToken = "new-access-token",
                ExpiresIn = 3600
            });
            var context = CreateContext(Now.AddMinutes(1));

            await CreateEvents(refreshService).ValidatePrincipal(context);

            Assert.Equal("new-access-token", context.Properties.GetTokenValue("access_token"));
            Assert.Equal("refresh-token", context.Properties.GetTokenValue("refresh_token"));
            Assert.Equal("old-id-token", context.Properties.GetTokenValue("id_token"));
        }

        [Fact]
        public async Task ValidatePrincipal_RefreshTokenRejected_RejectsPrincipalAndSignsOut()
        {
            var refreshService = CreateRefreshService(new RefreshedTokensServiceModel { IsRefreshTokenRejected = true });
            var authenticationService = Substitute.For<IAuthenticationService>();
            var context = CreateContext(Now.AddMinutes(1), authenticationService: authenticationService);

            await CreateEvents(refreshService).ValidatePrincipal(context);

            Assert.Null(context.Principal);
            await authenticationService.Received(1).SignOutAsync(context.HttpContext, CookiesScheme, Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task ValidatePrincipal_IdentityUnreachable_KeepsSessionAndTokens()
        {
            var refreshService = CreateRefreshService(new RefreshedTokensServiceModel());
            var authenticationService = Substitute.For<IAuthenticationService>();
            var context = CreateContext(Now.AddMinutes(1), authenticationService: authenticationService);

            await CreateEvents(refreshService).ValidatePrincipal(context);

            Assert.NotNull(context.Principal);
            Assert.False(context.ShouldRenew);
            Assert.Equal("old-access-token", context.Properties.GetTokenValue("access_token"));
            await authenticationService.DidNotReceive().SignOutAsync(Arg.Any<HttpContext>(), Arg.Any<string>(), Arg.Any<AuthenticationProperties>());
        }

        [Fact]
        public async Task ValidatePrincipal_SessionWithoutRefreshToken_DoesNotRefresh()
        {
            var refreshService = Substitute.For<IAccessTokenRefreshService>();
            var context = CreateContext(Now.AddMinutes(-30), refreshToken: null);

            await CreateEvents(refreshService).ValidatePrincipal(context);

            await refreshService.DidNotReceive().RefreshAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
            Assert.NotNull(context.Principal);
            Assert.False(context.ShouldRenew);
        }

        private static AccessTokenRefreshCookieEvents CreateEvents(IAccessTokenRefreshService refreshService)
        {
            return new AccessTokenRefreshCookieEvents(refreshService, new FixedTimeProvider());
        }

        private static IAccessTokenRefreshService CreateRefreshService(RefreshedTokensServiceModel result)
        {
            var refreshService = Substitute.For<IAccessTokenRefreshService>();

            refreshService.RefreshAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(result);

            return refreshService;
        }

        private static CookieValidatePrincipalContext CreateContext(
            DateTimeOffset accessTokenExpiresAt,
            string refreshToken = "refresh-token",
            IAuthenticationService authenticationService = null)
        {
            var services = new ServiceCollection();

            services.AddSingleton(authenticationService ?? Substitute.For<IAuthenticationService>());

            var httpContext = new DefaultHttpContext { RequestServices = services.BuildServiceProvider() };

            var tokens = new List<AuthenticationToken>
            {
                new() { Name = "access_token", Value = "old-access-token" },
                new() { Name = "id_token", Value = "old-id-token" },
                new() { Name = "expires_at", Value = accessTokenExpiresAt.ToString("o", CultureInfo.InvariantCulture) }
            };

            if (refreshToken is not null)
            {
                tokens.Add(new AuthenticationToken { Name = "refresh_token", Value = refreshToken });
            }

            var properties = new AuthenticationProperties();

            properties.StoreTokens(tokens);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("sub", "user-id") }, CookiesScheme));
            var scheme = new AuthenticationScheme(CookiesScheme, null, typeof(CookieAuthenticationHandler));

            return new CookieValidatePrincipalContext(
                httpContext,
                scheme,
                new CookieAuthenticationOptions(),
                new AuthenticationTicket(principal, properties, CookiesScheme));
        }

        private sealed class FixedTimeProvider : TimeProvider
        {
            public override DateTimeOffset GetUtcNow() => Now;
        }
    }
}
