using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Buyer.Web.Shared.DependencyInjection;
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

builder.Services.AddRazorPages();

builder.Services.AddLocalization();

builder.Services.AddCultureRouteConstraint();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(HttpWebGlobalExceptionFilter));
}).AddNewtonsoftJson();

builder.Services.RegisterClientAccountDependencies(builder.Configuration);

builder.Services.RegisterLocalizationDependencies();

builder.Services.RegisterOrdersAreaDependencies();

builder.Services.RegisterNewsDependencies();

builder.Services.RegisterGeneralDependencies();

builder.Services.RegisterDependencies();

builder.Services.RegisterApiExtensionsDependencies();

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.ConigureHealthChecks(builder.Configuration);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseGeneralException();

app.UseResponseCompression();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

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
                pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Home}/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area:exists=Home}/{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

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
