using Foundation.Extensions.Definitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IEnumerable<string> pathsToIgnore,
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

                if (string.IsNullOrWhiteSpace(configuration["OpenTelemetryCollectorUrl"]) is false)
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
