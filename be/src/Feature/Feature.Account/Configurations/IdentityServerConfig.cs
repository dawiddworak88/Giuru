using Foundation.ApiExtensions.Definitions;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
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
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource(ApiExtensionsConstants.AllScopes, ApiExtensionsConstants.AllScopes)
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var clientsList = new List<Client>();

            var clientsConfiguration = configuration.GetSection("Account")?.GetSection("Clients")?.GetChildren();

            if (clientsConfiguration != null)
            {
                foreach (var clientConfiguration in clientsConfiguration)
                {
                    var client = new Client
                    {
                        ClientId = clientConfiguration.GetValue<string>("ClientId"),
                        ClientSecrets = { new Secret(clientConfiguration.GetValue<string>("ClientSecret").Sha256()) },
                        AllowedGrantTypes = GrantTypes.Code,
                        RequireConsent = false,
                        RequirePkce = true,
                        RedirectUris = 
                        { 
                            $"{Definitions.AccountConstants.HttpsScheme}://{clientConfiguration.GetValue<string>("Host")}{clientConfiguration.GetValue<string>("SignInOidc")}",
                            $"{Definitions.AccountConstants.HttpScheme}://{clientConfiguration.GetValue<string>("Host")}{clientConfiguration.GetValue<string>("SignInOidc")}"
                        },
                        PostLogoutRedirectUris = { $"{clientConfiguration.GetValue<string>("Host")}/{clientConfiguration.GetValue<string>("SignOutCallbackOidc")}" },
                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            ApiExtensionsConstants.AllScopes
                        },
                        AllowOfflineAccess = true
                    };

                    clientsList.Add(client);
                }
            }

            return clientsList;
        }
            
    }
}
