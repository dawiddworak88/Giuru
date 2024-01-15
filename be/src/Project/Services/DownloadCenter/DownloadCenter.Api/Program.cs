using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Sinks.Logz.Io;
using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Options;
using Foundation.Extensions.Filters;
using Foundation.Localization.Extensions;
using Foundation.Localization.Definitions;
using Foundation.Account.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using DownloadCenter.Api.DependencyInjection;
using Foundation.EventBus.Abstractions;
using DownloadCenter.Api.IntegrationEvents;
using Foundation.Extensions.Formatters;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
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

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters().AddCsvSerializerFormatters();

builder.Services.AddLocalization();

builder.Services.AddApiVersioning();

builder.Services.RegisterDownloadCenterApiDependencies();

builder.Services.RegisterDatabaseDependencies(builder.Configuration);

builder.Services.RegisterApiAccountDependencies(builder.Configuration);

builder.Services.RegisterEventBus(builder.Configuration);

builder.Services.ConfigureSettings(builder.Configuration);

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

builder.Services.ConigureHealthChecks(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DownloadCenter API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();

app.ConfigureDatabaseMigrations(builder.Configuration);

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DownloadCenter API");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomHeaderRequestLocalizationProvider(builder.Configuration, app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

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

var eventBus = app.Services.GetService<IEventBus>();

eventBus.Subscribe<UpdatedFileIntegrationEvent, IIntegrationEventHandler<UpdatedFileIntegrationEvent>>();

app.Run();