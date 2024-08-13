using Giuru.MockAuth.Definitions;
using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Giuru.MockAuth.Configurations
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
           new List<IdentityResource>
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", new[] { JwtClaimTypes.Role })
           };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("all")
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Audience,
                        JwtClaimTypes.Role
                    },
                    Scopes = new List<string>
                    {
                        "all"
                    }
                }
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("all", "all")
            };

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
            {
                ClientId = "apiClient",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets =
                {
                    new Secret("apiSecret".Sha256())
                },
                AllowedScopes = { "all" }
            }
        };
        }
    }
}
