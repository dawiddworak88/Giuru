using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Products.SearchModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Catalog.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogContext>();

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }

        public static void RegisterSearchDependencies(this IServiceCollection services, IConfiguration configuration)
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
                                    .Category(ctgr => ctgr.Name("isActive")))))));
            }

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
