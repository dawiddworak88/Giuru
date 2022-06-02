using Foundation.Account.Definitions;
using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Foundation.Localization.Definitions;
using Identity.Api.Areas.Accounts.Configurations;
using Identity.Api.Areas.Accounts.Services.ProfileServices;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Configurations;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Repositories.AppSecrets;
using Identity.Api.Services.Organisations;
using Identity.Api.Services.Tokens;
using Identity.Api.Services.Users;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RabbitMQ.Client;
using System;
using System.Reflection;
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
                var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (authority, resource, scope) =>
                {
                    var context = new AuthenticationContext(authority);
                    var credentials = new ClientCredential(configuration.GetValue<string>("AzureKeyVaultClientId"), configuration.GetValue<string>("AzureKeyVaultClientSecret"));
                    var authenticationResult = await context.AcquireTokenAsync(resource, credentials);
                    return authenticationResult.AccessToken;
                }));

                builder.AddSigningCredential(new X509Certificate2(Convert.FromBase64String(client.GetSecretAsync(configuration.GetValue<string>("AzureKeyVaultGiuruIdentityServer4Certificate")).Result.Value), configuration.GetValue<string>("AzureKeyVaultGiuruIdentityServer4CertificatePassword")));
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
        }

        public static void RegisterAccountsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsersService, UsersService>();
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(configuration["EventBusConnection"]),
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMq(sp, rabbitMqPersistentConnection, logger, eventBusSubcriptionsManager, Assembly.GetExecutingAssembly().GetName().Name, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
