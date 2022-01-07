using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.Configurations
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource(ApiExtensionsConstants.AllScopes)
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Audience
                    },
                    Scopes = new List<string>
                    {
                        ApiExtensionsConstants.AllScopes
                    }
                }
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope(ApiExtensionsConstants.AllScopes, ApiExtensionsConstants.AllScopes)
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var clientsList = new List<Client>();

            var clientsConfiguration = configuration["Clients"]?.Split(";");

            if (clientsConfiguration != null)
            {
                foreach (var clientConfiguration in clientsConfiguration)
                {
                    var clientParameters = clientConfiguration.Split("&");

                    var client = new Client
                    {
                        ClientId = clientParameters[0],
                        ClientSecrets = { new Secret(clientParameters[1].Sha256()) },
                        AllowedGrantTypes = GrantTypes.Code,
                        RequireConsent = false,
                        RequirePkce = true,
                        RedirectUris = 
                        { 
                            $"{AccountConstants.HttpsScheme}://{clientParameters[2]}/signin-oidc",
                            $"{AccountConstants.HttpScheme}://{clientParameters[2]}/signin-oidc"
                        },
                        PostLogoutRedirectUris = {
                            $"{AccountConstants.HttpsScheme}://{clientParameters[2]}/signout-callback-oidc",
                            $"{AccountConstants.HttpScheme}://{clientParameters[2]}/signout-callback-oidc"                        
                        },
                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            ApiExtensionsConstants.AllScopes
                        },
                        AllowOfflineAccess = false,
                        AlwaysIncludeUserClaimsInIdToken = true
                    };

                    clientsList.Add(client);
                }
            }

            return clientsList;
        }
            
    }
}
