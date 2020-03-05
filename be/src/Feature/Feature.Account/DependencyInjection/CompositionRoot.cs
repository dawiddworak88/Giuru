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

            //services.AddIdentityServer()
            //    .AddSigningCredential(cert)
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryApiResources(Config.GetApiResources())
            //    .AddInMemoryClients(Config.GetClients(stsConfig))
            //    .AddAspNetIdentity<ApplicationUser>()
            //    .AddProfileService<IdentityWithAdditionalClaimsProfileService>();

            services.AddAuthentication()
            .AddOpenIdConnect("aad", "Login with Azure AD", options =>
            {
                options.Authority = $"https://login.microsoftonline.com/common";
                options.TokenValidationParameters =
                        new TokenValidationParameters { ValidateIssuer = false };
                options.ClientId = "99eb0b9d-ca40-476e-b5ac-6f4c32bfb530";
                options.CallbackPath = "/signin-oidc";
            });
        }
    }
}