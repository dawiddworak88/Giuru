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
using System.Collections.Generic;
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

                var missing = new List<string>();
                if (string.IsNullOrWhiteSpace(keyVaultUrl)) missing.Add("AzureKeyVaultUrl");
                if (string.IsNullOrWhiteSpace(tenantId)) missing.Add("AzureKeyVaultTenantId");
                if (string.IsNullOrWhiteSpace(clientId)) missing.Add("AzureKeyVaultClientId");
                if (string.IsNullOrWhiteSpace(clientSecret)) missing.Add("AzureKeyVaultClientSecret");
                if (string.IsNullOrWhiteSpace(certificateSecretIdentifier)) missing.Add("AzureKeyVaultGiuruIdentityServer4Certificate");
                if (certificatePassword is null) missing.Add("AzureKeyVaultGiuruIdentityServer4CertificatePassword");

                if (missing.Count > 0)
                {
                    throw new InvalidOperationException(
                        $"Identity.Api is running in production but required Key Vault configuration is missing: {string.Join(", ", missing)}. " +
                        "Add these values to GitOps secrets for Identity.Api.");
                }

                if (!Uri.TryCreate(keyVaultUrl, UriKind.Absolute, out var vaultUri))
                {
                    throw new InvalidOperationException(
                        $"AzureKeyVaultUrl is not a valid absolute URI: '{keyVaultUrl}'.");
                }

                if (!Uri.TryCreate(certificateSecretIdentifier, UriKind.Absolute, out var certificateSecretUri))
                {
                    throw new InvalidOperationException(
                        $"AzureKeyVaultGiuruIdentityServer4Certificate is not a valid absolute URI: '{certificateSecretIdentifier}'.");
                }

                KeyVaultSecretIdentifier secretId;
                try
                {
                    secretId = new KeyVaultSecretIdentifier(certificateSecretUri);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(
                        $"AzureKeyVaultGiuruIdentityServer4Certificate is not a valid Key Vault secret identifier: '{certificateSecretIdentifier}'.", ex);
                }

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                var secretClient = new SecretClient(vaultUri, credential);
                var secret = secretClient.GetSecret(secretId.Name, secretId.Version).Value;

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
