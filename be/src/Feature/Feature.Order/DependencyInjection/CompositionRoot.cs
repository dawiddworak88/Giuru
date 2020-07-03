using Feature.Order.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Order.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrderDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
