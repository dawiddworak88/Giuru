using Foundation.Schema.Services.SchemaServices;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Schema.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisteSchemaDependencies(this IServiceCollection services)
        {
            services.AddScoped<SchemaServiceFactory>();
        }
    }
}
