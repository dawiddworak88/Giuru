using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ModelBuilders;
using Identity.Api.Areas.Accounts.ViewModels;
using Identity.Api.ModelBuilders.SignInForm;
using Identity.Api.Shared.Footers.ModelBuilders;
using Identity.Api.Shared.Headers.ModelBuilders;
using Identity.Api.ViewModels.SignInForm;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Identity.Api.Shared.Configurations;
using System.Security.Cryptography.X509Certificates;
using System;
using Microsoft.Azure.KeyVault;
using Identity.Api.Areas.Accounts.Services.ProfileServices;
using Identity.Api.Areas.Accounts.Configurations;
using Identity.Api.Infrastructure.Accounts.Entities;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.v1.Areas.Accounts.Repositories.AppSecrets;
using Identity.Api.v1.Areas.Accounts.Services.TokenServices;
using Identity.Api.Infrastructure;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Services;

namespace Identity.Api.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountDependencies(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            // Configure identity server 4
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options => {

                options.IssuerUri = "null";
                options.UserInteraction.LoginUrl = "/Accounts/SignIn";
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

            services.AddAuthentication();

            // Register services
            services.AddScoped<v1.Areas.Accounts.Services.TokenServices.ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAppSecretRepository, AppSecretRepository>();
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IComponentModelBuilder<SignInComponentModel, SignInViewModel>, SignInModelBuilder>();
            services.AddScoped<IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel>, SignInFormModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
        }
    }
}
