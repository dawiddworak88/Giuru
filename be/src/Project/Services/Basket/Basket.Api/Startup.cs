using System;
using System.IO;
using System.Reflection;
using Basket.Api.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Foundation.EventBusRabbitMq;
using Foundation.EventLog.DependencyInjection;
using Foundation.Extensions.Filters;
using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace Basket.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddNewtonsoftJson();

            services.AddLocalization();

            services.RegisterApiAccountDependencies(this.Configuration);

            services.RegisterBasketApiDependencies();

            services.AddApiVersioning();

            services.ConfigureSettings(this.Configuration);

            services.AddSingleton(sp =>
            {
                var configuration = ConfigurationOptions.Parse(this.Configuration["ConnectionString"], true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });

            services.RegisterEventBus(this.Configuration);

            services.RegisterEventLogDependencies();

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                return new DefaultRabbitMQPersistentConnection(factory, logger, int.Parse(Configuration["EventBusRetryCount"]));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IOptionsMonitor<LocalizationSettings> localizationSettings)
        {
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

            app.UseCustomHeaderRequestLocalizationProvider(localizationSettings);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
