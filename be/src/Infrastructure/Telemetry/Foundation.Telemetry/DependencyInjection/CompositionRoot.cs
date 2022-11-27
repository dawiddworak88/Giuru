using Foundation.Extensions.Definitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Reflection;

namespace Foundation.Telemetry.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOpenTelemetry(
            this IServiceCollection services,
            IConfiguration configuration,
            string name,
            bool enableRedisInstrumentation,
            bool enableSqlClientInstrumentation,
            bool enableEntityFrameworkInstrumentation,
            bool enableHttpClientInstrumentation,
            bool enableAspNetCoreInstrumentation,
            string environmentName)
        {
            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource(name)
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(name));

                if (enableSqlClientInstrumentation)
                {
                    tracerProviderBuilder.AddSqlClientInstrumentation();
                }

                if (enableEntityFrameworkInstrumentation)
                {
                    tracerProviderBuilder.AddEntityFrameworkCoreInstrumentation();
                }

                if (enableRedisInstrumentation)
                {
                    tracerProviderBuilder.AddRedisInstrumentation();
                }

                if (enableHttpClientInstrumentation)
                {
                    tracerProviderBuilder.AddHttpClientInstrumentation();
                }

                if (enableAspNetCoreInstrumentation)
                {
                    tracerProviderBuilder.AddAspNetCoreInstrumentation();
                }

                if (environmentName == EnvironmentConstants.DevelopmentEnvironmentName
                    || string.IsNullOrWhiteSpace(configuration["OpenTelemetryCollectorUrl"]))
                {
                    tracerProviderBuilder.AddConsoleExporter();
                }
                else
                {
                    tracerProviderBuilder.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(configuration["OpenTelemetryCollectorUrl"]);
                    });
                }
            });
        }
    }
}
