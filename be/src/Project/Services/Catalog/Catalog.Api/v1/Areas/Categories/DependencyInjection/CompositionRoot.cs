using Catalog.Api.v1.Areas.Categories.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Categories.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterCategoryDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
