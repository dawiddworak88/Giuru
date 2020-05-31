using Foundation.Taxonomy.Services.TaxonomyServices;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Taxonomy.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterTaxonomyDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITaxonomyService, TaxonomyService>();
        }
    }
}
