using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Sinks.Logz.Io;
using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Foundation.Localization.Definitions;
using Foundation.PageContent.DependencyInjection;
using Identity.Api.Infrastructure.DependencyInjection;
using Identity.Api.DependencyInjection;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.DependencyInjection;
using Identity.Api.Areas.Accounts.DependencyInjection;
using Identity.Api.Areas.Home.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Foundation.Security.DependencyInjection;
using Microsoft.Extensions.Options;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Foundation.Mailing.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Foundation.Media.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Foundation.Telemetry.DependencyInjection;
using Foundation.ApiExtensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Foundation.Extensions.Filters;
using Foundation.Extensions.Formatters;
using Microsoft.Extensions.Logging;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

if (!builder.Environment.IsDevelopment())
{
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration.MinimumLevel.Warning();
        loggerConfiguration.MinimumLevel.Override("IdentityServer4", LogEventLevel.Debug);
        loggerConfiguration.Enrich.WithProperty("ApplicationContext", Assembly.GetExecutingAssembly().GetName().Name);
        loggerConfiguration.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName);
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


    builder.Services.AddOpenTelemetryTracing(
        builder.Configuration["OpenTelemetryTracingCollectorUrl"],
        Assembly.GetExecutingAssembly().GetName().Name,
        false,
        false,
        true,
        false,
        true,
        new[] { "/hc", "/liveness" });

    builder.Services.AddOpenTelemetryMetrics(
        builder.Configuration["OpenTelemetryMetricsCollectorUrl"],
        Assembly.GetExecutingAssembly().GetName().Name,
        true,
        true);
}

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
    options.RespectBrowserAcceptHeader = true;
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters().AddCsvSerializerFormatters();

builder.Services.RegisterLocalizationDependencies();

builder.Services.RegisterApiExtensionsDependencies();

builder.Services.RegisterFoundationMediaDependencies();

builder.Services.RegisterDatabaseDependencies(builder.Configuration);

builder.Services.RegisterBaseAccountDependencies();

builder.Services.RegisterAccountDependencies(builder.Configuration, builder.Environment.EnvironmentName != EnvironmentConstants.DevelopmentEnvironmentName);

builder.Services.RegisterGeneralDependencies();

builder.Services.RegisterAccountsViewsDependencies();

builder.Services.RegisterAccountsApiDependencies();

builder.Services.RegisterHomeViewsDependencies();

builder.Services.RegisterStrapiDependencies(builder.Configuration);

builder.Services.RegisterMailingDependencies(builder.Configuration);

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddApiVersioning();

builder.Services.ConigureHealthChecks(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseGeneralException();

app.UseResponseCompression();

app.UseGeneralStaticFiles();

var scope = app.Services.CreateAsyncScope();

app.ConfigureDatabaseMigrations(builder.Configuration, scope.ServiceProvider.GetService<IUserService>());

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

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.UseCustomHeaderRequestLocalizationProvider(builder.Configuration, app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

app.UseCustomRouteRequestLocalizationProvider(app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

app.UseSecurityHeaders(builder.Configuration);

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API");
    c.RoutePrefix = string.Empty;
});

app.MapControllerRoute(
    name: "localizedAreaRoute",
    pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "error",
    pattern: "{controller=Content}/{action=Error}/{errorId?}");

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
