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
                        tracerProviderBuilder.AddAspNetCoreInstrumentation(o =>
                        {
                            if (pathsToIgnore is not null && pathsToIgnore.Any())
                            {
                                var pathsToIgnoreList = pathsToIgnore.ToList();

                                o.Filter = context =>
                                {
                                    return !pathsToIgnoreList.Contains(context.Request.Path);
                                };
                            }
                        });
                    }

                    tracerProviderBuilder.AddOtlpExporter(o =>
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
                services.AddOpenTelemetryMetrics(meterProviderBuilder =>
                {
                    meterProviderBuilder
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(name));

                    if (enableHttpClientInstrumentation)
                    {
                        meterProviderBuilder.AddHttpClientInstrumentation();
                    }

                    if (enableAspNetCoreInstrumentation)
                    {
                        meterProviderBuilder.AddAspNetCoreInstrumentation();
                    }

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
