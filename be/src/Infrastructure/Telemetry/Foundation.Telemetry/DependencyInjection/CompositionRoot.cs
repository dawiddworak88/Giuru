using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Telemetry.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void AddOpenTelemetryTracing(
            this IServiceCollection services,
            string endpoint,
            string name,
            bool enableRedisInstrumentation,
            bool enableSqlClientInstrumentation,
            bool enableEntityFrameworkInstrumentation,
            bool enableHttpClientInstrumentation,
            bool enableAspNetCoreInstrumentation,
            IEnumerable<string> pathsToIgnore)
        {
            if (string.IsNullOrWhiteSpace(endpoint) is false)
            {
                //services.AddOpenTelemetryTracing(endpoint, name, enableRedisInstrumentation, enableHttpClientInstrumentation, enableSqlClientInstrumentation, enableEntityFrameworkInstrumentation, enableAspNetCoreInstrumentation, pathsToIgnore);

                services.AddOpenTelemetry().WithTracing(meterProviderBuilder =>
                {
                    meterProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name));
                    meterProviderBuilder.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(endpoint);
                    });
                });
            }
        }

        public static void AddOpenTelemetryMetrics(
            this IServiceCollection services,
            string endpoint, 
            string name,
            bool enableHttpClientInstrumentation,
            bool enableAspNetCoreInstrumentation)
        {
            if (string.IsNullOrWhiteSpace(endpoint) is false)
            {
                //services.AddOpenTelemetryMetrics(endpoint, name, enableHttpClientInstrumentation, enableAspNetCoreInstrumentation);

                services.AddOpenTelemetry().WithMetrics(meterProviderBuilder =>
                {
                    meterProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name));
                    meterProviderBuilder.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(endpoint);
                    });
                });
            }
        }

        public static void AddOpenTelemetrySerilogLogs(this LoggerConfiguration loggerConfiguration, string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint) is false)
            {
                loggerConfiguration.WriteTo.OpenTelemetry(endpoint);
            }
        }
    }
}
