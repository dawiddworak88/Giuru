using Client.Api.Infrastructure;
using Client.Api.Services.Applications;
using Client.Api.Services.Clients;
using Client.Api.Services.Groups;
using Client.Api.Services.Roles;
using Client.Api.Services.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Client.Api.Services.Addresses;
using Client.Api.Services.Fields;
using Client.Api.Services.FieldOptions;
using Client.Api.Services.FieldValues;
using Foundation.EventBus.Abstractions;
using Foundation.EventBusRabbitMq;
using Foundation.EventBus;
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
            services.AddScoped<IClientRolesService, ClientRolesService>();
            services.AddScoped<IClientsApplicationsService, ClientsApplicationsService>();
            services.AddScoped<IClientAccountManagersService, ClientAccountManagersService>();
            services.AddScoped<IClientAddressesService, ClientAddressesService>();
            services.AddScoped<IClientFieldsService,  ClientFieldsService>();
            services.AddScoped<IClientFieldOptionsService, ClientFieldOptionsService>();
            services.AddScoped<IClientFieldValuesService, ClientFieldValuesService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ClientContext>();

            services.AddDbContext<ClientContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(configuration["EventBusConnection"]),
                    DispatchConsumersAsync = true
                };

                factory.RequestedHeartbeat = TimeSpan.FromSeconds(int.Parse(configuration["EventBusRequestedHeartbeat"]));

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
