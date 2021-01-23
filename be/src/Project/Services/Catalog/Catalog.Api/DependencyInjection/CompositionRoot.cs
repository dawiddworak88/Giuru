using Catalog.Api.Repositories.Products.ProductIndexingRepositories;
using Catalog.Api.Repositories.Products.ProductSearchRepositories;
using Catalog.Api.Services.Categories;
using Catalog.Api.Services.Products;
using Catalog.Api.SearchModels.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using Foundation.Catalog.Infrastructure;

namespace Catalog.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCatalogApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddScoped<IProductIndexingRepository, ProductIndexingRepository>();
            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
            services.AddScoped<IProductsService, ProductsService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CatalogContext>();

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite().MigrationsAssembly("Catalog.Api")));
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
                                    .Category(ctgr => ctgr.Name("isActive"))
                                    .Category(ctgr => ctgr.Name("language")))))));
            }

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
