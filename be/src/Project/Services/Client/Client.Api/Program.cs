using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Sinks.Logz.Io;
using Foundation.Extensions.Filters;
using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Client.Api.DependencyInjection;
using Microsoft.Extensions.Options;
using Foundation.Localization.Definitions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
        loggerConfiguration.WriteTo.Http(hostingContext.Configuration["LogstashUrl"]);
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

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddLocalization();

builder.Services.RegisterApiAccountDependencies(builder.Configuration);

builder.Services.AddApiVersioning();

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.RegisterClientApiDependencies();

builder.Services.RegisterDatabaseDependencies(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.ConigureHealthChecks(builder.Configuration);

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API");
    c.RoutePrefix = string.Empty;
});

app.ConfigureDatabaseMigrations();

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

app.Run();
