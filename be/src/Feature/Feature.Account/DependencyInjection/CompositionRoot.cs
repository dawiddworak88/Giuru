using Feature.Account.Configurations;
using Feature.Account.Services;
using Foundation.ApiExtensions.Definitions;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Feature.Account.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
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

            builder.Services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            builder.AddDeveloperSigningCredential();

            var accountConfiguration = configuration.GetSection("Account")?.GetSection("AzureAd");

            services.AddAuthentication()
            .AddOpenIdConnect("AAD", "Azure AD", options =>
            {
                options.Authority = accountConfiguration?.GetValue<string>("Authority");
                options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false };
                options.ClientId = accountConfiguration?.GetValue<string>("ClientId");
                options.CallbackPath = accountConfiguration.GetValue<string>("CallbackPath");
            });
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

                options.SaveTokens = true;

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