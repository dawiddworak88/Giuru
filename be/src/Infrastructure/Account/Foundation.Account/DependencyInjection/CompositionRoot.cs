using Foundation.Account.Definitions;
using Foundation.Account.Services;
using Foundation.Extensions.Definitions;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public static void RegisterClientAccountDependencies(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.CorrelationCookie.SameSite = SameSiteMode.None;
                options.NonceCookie.SameSite = SameSiteMode.None;

                if (environment.IsDevelopment())
                {
                    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;   // tylko jeśli HTTP
                    options.NonceCookie.SecurePolicy = CookieSecurePolicy.None;
                }
                else
                {
                    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                }

                options.SignInScheme = "Cookies";
                options.Authority = configuration["IdentityUrl"];
                options.RequireHttpsMetadata = true;

                options.ClientId = configuration["ClientId"];
                options.ClientSecret = configuration["ClientSecret"];
                options.ResponseType = "code";
                options.UsePkce = true;

                options.CallbackPath = "/signin-oidc";

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
