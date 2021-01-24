using Foundation.Account.DependencyInjection;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Foundation.Extensions.Filters;
using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Ordering.Api.DependencyInjection;
using Ordering.Api.IntegrationEvents;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Reflection;

namespace Ordering.Api
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

            services.AddApiVersioning();

            services.ConfigureSettings(this.Configuration);

            services.AddMediatR(typeof(Startup));

            services.RegisterEventBus(this.Configuration);

            services.RegisterOrderingDatabaseDependencies(this.Configuration);

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering API", Version = "v1" });

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering API");
                c.RoutePrefix = string.Empty;
            });

            app.ConfigureOrderingDatabaseMigrations(this.Configuration);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomHeaderRequestLocalizationProvider(localizationSettings);

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BasketCheckoutAcceptedIntegrationEvent, IIntegrationEventHandler<BasketCheckoutAcceptedIntegrationEvent>>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
