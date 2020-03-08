using Feature.Account.Configurations;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Feature.Account.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddAspNetIdentity<ApplicationUser>();

            builder.AddDeveloperSigningCredential();

            var accountConfiguration = configuration.GetSection("Account")?.GetSection("AzureAd");

            services.AddAuthentication()
            .AddOpenIdConnect("AAD", "Azure AD", options =>
            {
                options.Authority = accountConfiguration?.GetValue<string>("Authority");
                options.TokenValidationParameters =
                        new TokenValidationParameters { ValidateIssuer = false };
                options.ClientId = accountConfiguration?.GetValue<string>("ClientId");
                options.CallbackPath = accountConfiguration.GetValue<string>("CallbackPath");
            });
        }
    }
}