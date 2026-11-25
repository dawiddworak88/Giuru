using Foundation.Account.Definitions;
using Foundation.Account.Services.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Account.Events
{
    // Refreshes the access token kept in the cookie shortly before it expires, so active users aren't signed out
    public class AccessTokenRefreshCookieEvents : CookieAuthenticationEvents
    {
        private readonly IAccessTokenRefreshService accessTokenRefreshService;
        private readonly TimeProvider timeProvider;

        public AccessTokenRefreshCookieEvents(
            IAccessTokenRefreshService accessTokenRefreshService,
            TimeProvider timeProvider)
        {
            this.accessTokenRefreshService = accessTokenRefreshService;
            this.timeProvider = timeProvider;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var refreshToken = context.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);

            // Sessions started before refresh tokens were enabled don't have one and end with their access token
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            var now = this.timeProvider.GetUtcNow();

            if (IsAccessTokenValidLongEnough(context.Properties.GetTokenValue(AccountConstants.Tokens.ExpiresAt), now))
            {
                return;
            }

            var tokens = await this.accessTokenRefreshService.RefreshAsync(refreshToken, context.HttpContext.RequestAborted);

            if (tokens.IsRefreshTokenRejected)
            {
                // The refresh token expired or was revoked, or the user was deactivated, so the session is over
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(context.Scheme.Name);

                return;
            }

            // Identity couldn't be reached, so keep the current tokens and try again on the next request
            if (tokens.Succeeded is false)
            {
                return;
            }

            var storedTokens = context.Properties.GetTokens().ToDictionary(token => token.Name, token => token.Value);

            storedTokens[OpenIdConnectParameterNames.AccessToken] = tokens.AccessToken;
            storedTokens[AccountConstants.Tokens.ExpiresAt] = now.AddSeconds(tokens.ExpiresIn).ToString("o", CultureInfo.InvariantCulture);

            if (string.IsNullOrWhiteSpace(tokens.RefreshToken) is false)
            {
                storedTokens[OpenIdConnectParameterNames.RefreshToken] = tokens.RefreshToken;
            }

            if (string.IsNullOrWhiteSpace(tokens.IdentityToken) is false)
            {
                storedTokens[OpenIdConnectParameterNames.IdToken] = tokens.IdentityToken;
            }

            context.Properties.StoreTokens(storedTokens.Select(token => new AuthenticationToken { Name = token.Key, Value = token.Value }));

            context.ShouldRenew = true;
        }

        private static bool IsAccessTokenValidLongEnough(string expiresAt, DateTimeOffset now)
        {
            if (DateTimeOffset.TryParse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiration) is false)
            {
                return false;
            }

            return expiration - now > AccountConstants.TokenLifetimes.RefreshAccessTokenBeforeExpiration;
        }
    }
}
