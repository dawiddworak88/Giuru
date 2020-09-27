using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Foundation.Search.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterSearchDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticsearchUrl"];
            var defaultIndex = configuration["ElasticsearchIndex"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex).DefaultDisableIdInference();

            var client = new ElasticClient(settings);
            
            if (!client.Indices.Exists(defaultIndex).Exists)
            {
                client.Indices.Create(defaultIndex);
            }

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
