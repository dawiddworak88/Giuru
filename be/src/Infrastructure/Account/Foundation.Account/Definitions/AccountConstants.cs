using System;

namespace Foundation.Account.Definitions
{
    public static class AccountConstants
    {
        public struct Audiences
        {
            public static readonly string All = "all";
        }

        public struct ApiNames
        {
            public static readonly string All = "all";
        }

        public struct Schemes
        {
            public static readonly string HttpsScheme = "https";
            public static readonly string HttpScheme = "http";
        }

        public struct Claims
        {
            public static readonly string OrganisationIdClaim = "OrganisationId";
        }

        public struct Roles
        {
            public static readonly string Seller = "Seller";
        }

        public struct TokenLifetimes
        {
            public static readonly int DefaultTokenLifetimeInDays = 1;
            public static readonly int DefaultTokenLifetimeInSeconds = 1 * 24 * 60 * 60;
            public static readonly int AccessTokenLifetimeInSeconds = 60 * 60;
            public static readonly int SlidingRefreshTokenLifetimeInSeconds = 7 * 24 * 60 * 60;
            public static readonly int AbsoluteRefreshTokenLifetimeInSeconds = 30 * 24 * 60 * 60;
            public static readonly TimeSpan RefreshAccessTokenBeforeExpiration = TimeSpan.FromMinutes(5);
        }

        public struct Tokens
        {
            public static readonly string ExpiresAt = "expires_at";
        }

        public struct AuthenticationSchemes
        {
            public static readonly string Cookies = "Cookies";
            public static readonly string OpenIdConnect = "oidc";
        }

        public struct IdentityResources
        {
            public static readonly string Roles = "roles";
        }

        public struct ApiResources
        {
            public static readonly string All = "all";
        }

        public struct Scopes
        {
            public static readonly string Roles = "roles";
            public static readonly string All = "all";
        }
    }
}
