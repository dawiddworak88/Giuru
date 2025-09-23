using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Sinks.Logz.Io;
using Foundation.Extensions.Filters;
using Catalog.BackgroundTasks.DependencyInjection;
using Foundation.Catalog.DependencyInjection;
using Foundation.EventBus.Abstractions;
using Catalog.BackgroundTasks.IntegrationEvents;
using Microsoft.Extensions.Options;
using Foundation.Localization.Definitions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Foundation.Telemetry.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

if (!builder.Environment.IsDevelopment())
{
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    {
        loggerConfiguration.MinimumLevel.Warning();
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
        false,
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson();

builder.Services.AddLocalization();

builder.Services.AddApiVersioning();

builder.Services.RegisterDatabaseDependencies(builder.Configuration);

builder.Services.RegisterSearchDependencies(builder.Configuration);

builder.Services.RegisterEventBus(builder.Configuration);

builder.Services.RegisterCatalogBaseDependencies();

builder.Services.RegisterCatalogBackgroundTasksDependencies();

builder.Services.ConigureHealthChecks(builder.Configuration);

builder.Services.ConfigureSettings(builder.Configuration);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomHeaderRequestLocalizationProvider(builder.Configuration, app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

var eventBus = app.Services.GetService<IEventBus>();

eventBus.Subscribe<RebuildCatalogSearchIndexIntegrationEvent, IIntegrationEventHandler<RebuildCatalogSearchIndexIntegrationEvent>>();
eventBus.Subscribe<RebuildCategorySchemasIntegrationEvent, IIntegrationEventHandler<RebuildCategorySchemasIntegrationEvent>>();
eventBus.Subscribe<RebuildCategoryProductsIntegrationEvent, IIntegrationEventHandler<RebuildCategoryProductsIntegrationEvent>>();
eventBus.Subscribe<InventoryProductsAvailableQuantityUpdateIntegrationEvent, IIntegrationEventHandler<InventoryProductsAvailableQuantityUpdateIntegrationEvent>>();
eventBus.Subscribe<OutletProductsAvailableQuantityUpdateIntegrationEvent, IIntegrationEventHandler<OutletProductsAvailableQuantityUpdateIntegrationEvent>>();

app.MapControllers();

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
