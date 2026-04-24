using Foundation.Account.Definitions;
using Foundation.Localization.Definitions;
using Foundation.Media.Configurations;
using Identity.Api.Areas.Accounts.Configurations;
using Identity.Api.Areas.Accounts.Services.ProfileServices;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Configurations;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Repositories.AppSecrets;
using Identity.Api.Repositories.GraphQl;
using Identity.Api.Services.Approvals;
using Identity.Api.Services.Organisations;
using Identity.Api.Services.Roles;
using Identity.Api.Services.TeamMembers;
using Identity.Api.Services.Tokens;
using Identity.Api.Services.UserApprovals;
using Identity.Api.Services.Users;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography.X509Certificates;
using ITokenService = Identity.Api.Services.Tokens.ITokenService;

namespace Identity.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options => {

                options.IssuerUri = "null";
                options.UserInteraction.LoginUrl = "/Accounts/SignIn";
                options.UserInteraction.LogoutUrl = "/Accounts/SignOut";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryApiResources(IdentityServerConfig.Apis)
            .AddInMemoryClients(IdentityServerConfig.GetClients(configuration))
            .AddAspNetIdentity<ApplicationUser>();

            builder.Services.AddScoped<IProfileService, ProfileService>();

            if (isProduction)
            {
                var keyVaultUrl = configuration.GetValue<string>("AzureKeyVaultUrl");
                var tenantId = configuration.GetValue<string>("AzureKeyVaultTenantId");
                var clientId = configuration.GetValue<string>("AzureKeyVaultClientId");
                var clientSecret = configuration.GetValue<string>("AzureKeyVaultClientSecret");
                var certificateSecretIdentifier = configuration.GetValue<string>("AzureKeyVaultGiuruIdentityServer4Certificate");
                var certificatePassword = configuration.GetValue<string>("AzureKeyVaultGiuruIdentityServer4CertificatePassword");

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);

                KeyVaultSecret secret;
                if (Uri.TryCreate(certificateSecretIdentifier, UriKind.Absolute, out var secretUri))
                {
                    var segments = secretUri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    var name = segments[1];
                    var version = segments.Length > 2 ? segments[2] : null;
                    secret = secretClient.GetSecret(name, version).Value;
                }
                else
                {
                    secret = secretClient.GetSecret(certificateSecretIdentifier).Value;
                }

                var certificate = X509CertificateLoader.LoadPkcs12(Convert.FromBase64String(secret.Value), certificatePassword);
                builder.AddSigningCredential(certificate);
            }
            else
            {
                builder.AddDeveloperSigningCredential();
            }

            services.AddAuthentication()
                .AddIdentityServerAuthentication("IsToken", options =>
                {
                    options.Authority = configuration.GetValue<string>("IdentityUrl");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = AccountConstants.ApiNames.All;
                });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAppSecretRepository, AppSecretRepository>();
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<LocalizationSettings>(configuration);
            services.Configure<MediaAppSettings>(configuration);
        }

        public static void RegisterAccountsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IApprovalsService, ApprovalsService>();
            services.AddScoped<IUserApprovalsService, UserApprovalsService>();
            services.AddScoped<IGraphQlRepository, GraphQlRepository>();
        }
    }
}
