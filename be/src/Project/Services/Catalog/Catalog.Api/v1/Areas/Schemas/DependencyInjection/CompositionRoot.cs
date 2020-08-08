using Catalog.Api.v1.Areas.Schemas.Services.SchemaServices;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Schemas.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisteSchemaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISchemaService, SchemaService>();
        }
    }
}
