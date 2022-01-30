using Foundation.Account.Definitions;
using Foundation.Account.Services;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

                    options.Audience = AccountConstants.Audiences.All;
                });
        }

        public static void RegisterClientAccountDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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

                options.TokenValidationParameters = new TokenValidationParameters 
                { 
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
                options.SaveTokens = true;

                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                options.Scope.Add(AccountConstants.Scopes.All);
                options.Scope.Add(AccountConstants.Scopes.Roles);

                options.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role, JwtClaimTypes.Role);
            });
        }
    }
}
