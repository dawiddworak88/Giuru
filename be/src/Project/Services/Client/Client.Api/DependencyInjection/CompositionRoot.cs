using Client.Api.Infrastructure;
using Client.Api.IntegrationEvents;
using Client.Api.IntegrationEventsHandlers;
using Client.Api.Services.Applications;
using Client.Api.Services.Clients;
using Client.Api.Services.Groups;
using Foundation.EventBus;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Client.Api.Services.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Reflection;

namespace Client.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IClientGroupsService, ClientGroupsService>();
            services.AddScoped<IClientsApplicationsService, ClientsApplicationsService>();
            services.AddScoped<IClientAccountManagersService, ClientAccountManagersService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ClientContext>();

            services.AddDbContext<ClientContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<ClientApplicationIntegrationEvent>, ClientApplicationIntegrationEventHandler>();

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(configuration["EventBusConnection"]),
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMq(sp, rabbitMqPersistentConnection, logger, eventBusSubcriptionsManager, Assembly.GetExecutingAssembly().GetName().Name, int.Parse(configuration["EventBusRetryCount"]));
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
