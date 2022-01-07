using Foundation.Catalog.Repositories.Products.ProductIndexingRepositories;
using Foundation.Catalog.Repositories.ProductSearchRepositories;
using Foundation.Catalog.SearchModels.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Foundation.Catalog.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCatalogBaseDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductIndexingRepository, ProductIndexingRepository>();
        }

        public static void RegisterCatalogRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
        }

        public static void RegisterCatalogSearchDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticsearchUrl"];
            var defaultIndex = configuration["ElasticsearchIndex"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex).DefaultDisableIdInference();

            var client = new ElasticClient(settings);

            if (!client.Indices.Exists(defaultIndex).Exists)
            {
                client.Indices.Create(defaultIndex, c => c
                    .Map<ProductSearchModel>(m => m
                        .AutoMap()
                        .Properties(p => p
                            .Completion(cmpl => cmpl
                                .Name(n => n.NameSuggest)
                                .Contexts(ctxs => ctxs
                                    .Category(ctgr => ctgr.Name("isActive"))
                                    .Category(ctgr => ctgr.Name("primaryProductIdHasValue"))))
                            .Completion(cmpl => cmpl
                                .Name(n => n.BrandNameSuggest)
                                .Contexts(ctxs => ctxs
                                    .Category(ctgr => ctgr.Name("isActive"))
                                    .Category(ctgr => ctgr.Name("primaryProductIdHasValue"))))
                            .Completion(cmpl => cmpl
                                .Name(n => n.CategoryNameSuggest)
                                .Contexts(ctxs => ctxs
                                    .Category(ctgr => ctgr.Name("isActive"))
                                    .Category(ctgr => ctgr.Name("language")))))));
            }

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
