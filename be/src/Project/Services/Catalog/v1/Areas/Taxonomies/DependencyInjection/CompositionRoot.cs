using Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Taxonomies.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterTaxonomyDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITaxonomyService, TaxonomyService>();
        }
    }
}
