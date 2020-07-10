using Feature.ImportOrder.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.ImportOrder.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterImportOrderDependencies(this IServiceCollection services)
        {
            services.AddScoped<IImportOrderServiceFactory, ImportOrderServiceFactory>();
        }
    }
}
