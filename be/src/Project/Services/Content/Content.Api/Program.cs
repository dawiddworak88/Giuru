using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Logz.Io;

namespace Content.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .UseSerilog((hostingContext, loggerConfiguration) =>
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
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
