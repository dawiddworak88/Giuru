using Foundation.Account.Services;
using Foundation.ApiExtensions.Definitions;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Foundation.Account.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterBaseAccountDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        }

        public static void RegisterApiAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration.GetValue<string>("IdentityUrl");
                    options.RequireHttpsMetadata = false;

                    options.Audience = ApiExtensionsConstants.AllScopes;
                });
        }

        public static void RegisterClientAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = configuration["IdentityUrl"];
                options.RequireHttpsMetadata = false;

                options.ClientId = configuration["ClientId"];
                options.ClientSecret = configuration["ClientSecret"];
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "email" };
                options.SaveTokens = true;

                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                options.Scope.Add(ApiExtensionsConstants.AllScopes);
            });
        }
    }
}
