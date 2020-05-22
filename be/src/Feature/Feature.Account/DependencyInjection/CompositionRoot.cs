using Feature.Account.Configurations;
using Feature.Account.Services.ProfileServices;
using Feature.Account.Services.TokenServices;
using Feature.Account.Services.UserServices;
using Foundation.ApiExtensions.Definitions;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using ITokenService = Feature.Account.Services.TokenServices.ITokenService;

namespace Feature.Account.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            // Configure identity server 4
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        public static void RegisterClientAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationConfiguration = configuration.GetSection("Authentication");

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = authenticationConfiguration.GetValue<string>("AuthenticationScheme");
                options.DefaultChallengeScheme = authenticationConfiguration.GetValue<string>("ChallengeScheme");
            })
            .AddCookie(authenticationConfiguration.GetValue<string>("AuthenticationScheme"))
            .AddOpenIdConnect(authenticationConfiguration.GetValue<string>("ChallengeScheme"), options =>
            {
                options.Authority = authenticationConfiguration.GetValue<string>("Authority");
                options.RequireHttpsMetadata = false;

                options.ClientId = authenticationConfiguration.GetValue<string>("ClientId");
                options.ClientSecret = authenticationConfiguration.GetValue<string>("ClientSecret");
                options.ResponseType = authenticationConfiguration.GetValue<string>("ResponseType");

                options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "email" };
                options.SaveTokens = true;

                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                options.Scope.Add(ApiExtensionsConstants.AllScopes);
            });
        }

        public static void RegisterApiAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationConfiguration = configuration.GetSection("Authentication");

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authenticationConfiguration?.GetValue<string>("Authority");
                    options.RequireHttpsMetadata = false;

                    options.Audience = ApiExtensionsConstants.AllScopes;
                });
        }
    }
}