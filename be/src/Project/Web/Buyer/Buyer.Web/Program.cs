using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Buyer.Web.Shared.DependencyInjection;
using Buyer.Web.Areas.Content.DependencyInjection;
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
using Buyer.Web.Areas.Orders.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Foundation.Extensions.Filters;
using Buyer.Web.Areas.News.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Foundation.Media.DependencyInjection;
using Buyer.Web.Areas.Clients.DependencyInjection;
using Buyer.Web.Areas.DownloadCenter.DependencyInjection;
using Foundation.Extensions.Definitions;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Foundation.Telemetry.DependencyInjection;
using Buyer.Web.Areas.Dashboard.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.MinimumLevel.Warning();
    loggerConfiguration.Enrich.WithProperty("ApplicationContext", typeof(Program).Namespace);
    loggerConfiguration.Enrich.FromLogContext();
    loggerConfiguration.WriteTo.Console();

    loggerConfiguration.AddOpenTelemetrySerilogLogs(hostingContext.Configuration["OpenTelemetryLogsCollectorUrl"]);

    if (!string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoToken"])
        && !string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoType"])
        && !string.IsNullOrWhiteSpace(hostingContext.Configuration["LogzIoDataCenterSubDomain"]))
    {
        loggerConfiguration.WriteTo.LogzIo(hostingContext.Configuration["LogzIoToken"],
            hostingContext.Configuration["LogzIoType"],
            new LogzioOptions
            {
                DataCenter = new LogzioDataCenter
                {
                    SubDomain = hostingContext.Configuration["LogzIoDataCenterSubDomain"]
                } 
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

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(HttpWebGlobalExceptionFilter));
}).AddNewtonsoftJson();

builder.Services.RegisterFoundationMediaDependencies();

builder.Services.RegisterClientAccountDependencies(builder.Configuration, builder.Environment);

builder.Services.RegisterLocalizationDependencies();

builder.Services.RegisterOrdersAreaDependencies();

builder.Services.RegisterClientsDependencies();

builder.Services.RegisterNewsDependencies();

builder.Services.RegisterDownloadCenterDependencies();

builder.Services.RegisterDashboardAreaDependencies();

builder.Services.RegisterContentDependencies();

builder.Services.RegisterGeneralDependencies();

builder.Services.RegisterDependencies(builder.Configuration);

builder.Services.RegisterStrapiDependencies(builder.Configuration);

builder.Services.RegisterApiExtensionsDependencies();

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddOpenTelemetryTracing(
    builder.Configuration["OpenTelemetryTracingCollectorUrl"],
    Assembly.GetExecutingAssembly().GetName().Name,
    false,
    false,
    false,
    true,
    true,
    new[] { "/hc", "/liveness" });

builder.Services.AddOpenTelemetryMetrics(
    builder.Configuration["OpenTelemetryMetricsCollectorUrl"],
    Assembly.GetExecutingAssembly().GetName().Name,
    true,
    true);

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

//app.MapControllerRoute(
//    name: "localizedAreaSlugRoute",
//    pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{slug?}",
//    defaults: new
//    {
//        area = "Content",
//        controller = "Content",
//        action = "Index"
//    }).RequireAuthorization();

app.MapControllerRoute(
    name: "localizedAreaRoute",
    pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Home}/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists=Home}/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.Run();
