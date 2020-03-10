using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Feature.Account.Configurations
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "daa95085-cde4-4732-91ca-cfdf6027de22",
                    ClientSecrets = { new Secret("145ade68-286d-4e4e-8895-557ba23098c9".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                    RedirectUris = { "https://localhost:44377/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44377/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowOfflineAccess = true
                }
            };
    }
}
