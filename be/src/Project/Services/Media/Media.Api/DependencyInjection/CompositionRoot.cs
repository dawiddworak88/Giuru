using Media.Api.Infrastructure;
using Media.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Media.Api.Services.Media;
using Media.Api.Services.Checksums;
using Foundation.EventBusRabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using Foundation.EventBus.Abstractions;
using Foundation.EventBus;
using System.Reflection;

namespace Media.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMediaApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IChecksumService, ChecksumService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MediaContext>();
            services.AddDbContext<MediaContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()), ServiceLifetime.Scoped);
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
