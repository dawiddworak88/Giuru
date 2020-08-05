using Identity.Api.Areas.Accounts.Configurations;
using Identity.Api.Areas.Accounts.Services.ProfileServices;
using Identity.Api.Areas.Accounts.Services.TokenServices;
using Identity.Api.Infrastructure.Accounts.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography.X509Certificates;
using ITokenService = Identity.Api.Areas.Accounts.Services.TokenServices.ITokenService;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Identity.Api.Infrastructure;

namespace Identity.Api.Areas.Accounts.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            // Configure identity server 4
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options => {

                options.UserInteraction.LoginUrl = "/";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
            .AddInMemoryApiResources(IdentityServerConfig.Apis)
            .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
            .AddAspNetIdentity<ApplicationUser>();

            builder.Services.AddScoped<IProfileService, ProfileService>();

            if (isProduction)
            {
                var azureKeyVaultConfiguration = configuration.GetSection("AzureKeyVault");

                var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (authority, resource, scope) =>
                {
                    var context = new AuthenticationContext(authority);
                    var credentials = new ClientCredential(azureKeyVaultConfiguration.GetValue<string>("ClientId"), azureKeyVaultConfiguration.GetValue<string>("ClientSecret"));
                    var authenticationResult = await context.AcquireTokenAsync(resource, credentials);
                    return authenticationResult.AccessToken;
                }));

                builder.AddSigningCredential(new X509Certificate2(Convert.FromBase64String(client.GetSecretAsync(azureKeyVaultConfiguration.GetValue<string>("GiuruIdentityServer4Certificate")).Result.Value)));
            }
            else
            {
                builder.AddDeveloperSigningCredential();
            }

            var accountConfiguration = configuration.GetSection("Account")?.GetSection("AzureAd");

            services.AddAuthentication()
            .AddOpenIdConnect("AAD", "Azure AD", options =>
            {
                options.Authority = accountConfiguration?.GetValue<string>("Authority");
                options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false };
                options.ClientId = accountConfiguration?.GetValue<string>("ClientId");
                options.CallbackPath = accountConfiguration.GetValue<string>("CallbackPath");
            });

            // Register services
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}