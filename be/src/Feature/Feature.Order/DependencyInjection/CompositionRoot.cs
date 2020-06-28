using Feature.Order.Repositories.ExtractOrderServices;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Order.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrderDependencies(this IServiceCollection services)
        {
            services.AddScoped<IExtractOrderRepository, ExtractOrderRepository>();
        }
    }
}
