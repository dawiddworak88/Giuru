using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Seller.Web.Shared.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;
using Foundation.ApiExtensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Serilog.Sinks.Logz.Io;
using Seller.Web.Areas.Orders.DependencyInjection;
using Seller.Web.Areas.Clients.DependencyInjection;
using Seller.Web.Areas.Products.DependencyInjection;
using Seller.Web.Areas.Settings.DependencyInjection;
using Seller.Web.Areas.Media.DependencyInjection;
using Foundation.Extensions.Filters;
using Microsoft.AspNetCore.Http;
using Seller.Web.Areas.Inventory.DependencyInjection;
using Foundation.Account.Definitions;
using Seller.Web.Areas.News.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Foundation.Media.DependencyInjection;
using Seller.Web.Areas.TeamMembers.DependencyInjection;
using Seller.Web.Areas.DownloadCenter.DependencyInjection;
using Foundation.Extensions.Definitions;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Seller.Web.Areas.Global.DependencyInjection;
using Foundation.Telemetry.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, config) =>
{
    config.AddEnvironmentVariables();
});

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.MinimumLevel.Warning();
    loggerConfiguration.Enrich.WithProperty("ApplicationContext", typeof(Program).Namespace);
    loggerConfiguration.Enrich.FromLogContext();
    loggerConfiguration.WriteTo.Console();

    if (!string.IsNullOrWhiteSpace(hostingContext.Configuration["LogstashUrl"]))
    {
        loggerConfiguration.WriteTo.Http(requestUri: hostingContext.Configuration["LogstashUrl"], queueLimitBytes: null);
    }

    if (!string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoToken"])
        && !string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoType"])
        && !string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoDataCenterSubDomain"]))
    {
        loggerConfiguration.WriteTo.LogzIo(hostingContext.Configuration["LogzIoToken"],
            hostingContext.Configuration["LogzIoType"],
            new LogzioOptions
            {
                DataCenterSubDomain = hostingContext.Configuration["LogzIoDataCenterSubDomain"]
            });
    }

    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddDataProtection().UseCryptographicAlgorithms(
    new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    }).PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(builder.Configuration["RedisUrl"]), $"{Assembly.GetExecutingAssembly().GetName().Name}-DataProtection-Keys");

builder.Services.AddRazorPages();

builder.Services.AddLocalization();

builder.Services.AddCultureRouteConstraint();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(HttpWebGlobalExceptionFilter));
}).AddNewtonsoftJson();

builder.Services.RegisterFoundationMediaDependencies();

builder.Services.RegisterClientAccountDependencies(builder.Configuration);

builder.Services.RegisterApiExtensionsDependencies();

builder.Services.RegisterLocalizationDependencies();

builder.Services.RegisterGeneralDependencies();

builder.Services.RegisterDependencies();

builder.Services.RegisterOrdersAreaDependencies();

builder.Services.RegisterClientsAreaDependencies();

builder.Services.RegisterInventoryAreaDependencies();

builder.Services.RegisterDownloadCenterAreaDependencies();

builder.Services.RegisterNewsAreaDependencies();

builder.Services.RegisterTeamMembersAreaDependencies();

builder.Services.RegisterProductsAreaDependencies();

builder.Services.RegisterSettingsAreaDependencies();

builder.Services.RegisterMediaAreaDependencies();

builder.Services.RegisterGlobalAreaDependencies();

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SellerOnly", policy => policy.RequireRole(AccountConstants.Roles.Seller));
});

builder.Services.RegisterOpenTelemetry(
    builder.Configuration,
    Assembly.GetExecutingAssembly().GetName().Name,
    false,
    false,
    false,
    true,
    true,
    new [] { "/hc", "/liveness" },
    builder.Environment.EnvironmentName);

builder.Services.ConigureHealthChecks(builder.Configuration);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseGeneralException();

app.UseResponseCompression();

if (app.Environment.EnvironmentName == EnvironmentConstants.DevelopmentEnvironmentName)
{
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Lax
    });
}
else
{
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        Secure = CookieSecurePolicy.Always
    });
}

app.UseGeneralStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomRouteRequestLocalizationProvider(app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

app.UseSecurityHeaders(builder.Configuration);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                name: "localizedAreaRoute",
                pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Orders}/{controller=Orders}/{action=Index}/{id?}").RequireAuthorization("SellerOnly");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area:exists=Orders}/{controller=Orders}/{action=Index}/{id?}").RequireAuthorization("SellerOnly");

    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("self")
    });
});

app.Run();
