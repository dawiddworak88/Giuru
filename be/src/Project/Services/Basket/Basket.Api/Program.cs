using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Sinks.Logz.Io;
using Foundation.Extensions.Filters;
using Foundation.EventBusRabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Basket.Api.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using Foundation.Localization.Definitions;

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
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
}).AddNewtonsoftJson();

builder.Services.AddLocalization();

builder.Services.RegisterApiAccountDependencies(builder.Configuration);

builder.Services.RegisterBasketApiDependencies();

builder.Services.AddApiVersioning();

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddSingleton(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration["RedisConnectionString"], true);

    configuration.ResolveDns = true;

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.RegisterEventBus(builder.Configuration);

builder.Services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory
    {
        Uri = new Uri(builder.Configuration["EventBusConnection"]),
        DispatchConsumersAsync = true
    };

    return new DefaultRabbitMQPersistentConnection(factory, logger, int.Parse(builder.Configuration["EventBusRetryCount"]));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
IdentityModelEventSource.ShowPII = true;

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomHeaderRequestLocalizationProvider(builder.Configuration, app.Services.GetService<IOptionsMonitor<LocalizationSettings>>());

app.ConfigureEventBus();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
