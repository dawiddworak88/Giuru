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
using Microsoft.IdentityModel.Tokens;
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
                    options.TokenValidationParameters.ValidIssuer = configuration.GetValue<string>("IdentityUrl");
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
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                if (environment.EnvironmentName == EnvironmentConstants.DevelopmentEnvironmentName)
                {
                    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
                    options.NonceCookie.SameSite = SameSiteMode.Lax;
                    options.NonceCookie.SecurePolicy = CookieSecurePolicy.None;
                }

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

                options.Events.OnRedirectToIdentityProvider = context =>
                {
                    if (string.Equals(context.Request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                        string.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.Headers["Location"] = "/Accounts/Account/SignOutNow";
                        context.Properties.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}";
                    }
                    else
                    {
                        context.Properties.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
                        context.Properties.Items.Add(OpenIdConnectDefaults.RedirectUriForCodePropertiesKey, context.ProtocolMessage.RedirectUri);
                        context.ProtocolMessage.State = options.StateDataFormat.Protect(context.Properties);
                        context.Response.StatusCode = (int)HttpStatusCode.Redirect;
                        context.Response.Headers["Location"] = context.ProtocolMessage.CreateAuthenticationRequestUrl();
                    }

                    context.HandleResponse();

                    return Task.CompletedTask;
                };
            });
        }
    }
}
