using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Logz.Io;
using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using Foundation.Extensions.Filters;
using Foundation.Account.DependencyInjection;
using Global.Api.DependencyInjection;
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

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddLocalization();

builder.Services.AddApiVersioning();

builder.Services.RegisterGlobalApiDependencies();

builder.Services.RegisterDatabaseDependencies(builder.Configuration);

builder.Services.RegisterApiAccountDependencies(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Global API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.ConigureHealthChecks(builder.Configuration);

builder.Services.RegisterOpenTelemetry(
    builder.Configuration,
    Assembly.GetExecutingAssembly().GetName().Name,
    false,
    false,
    true,
    false,
    true,
    new[] { "/hc", "/liveness" },
    builder.Environment.EnvironmentName);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Global API");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.ConfigureDatabaseMigrations(builder.Configuration);

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

app.Run();