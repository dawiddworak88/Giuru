using Catalog.Api.v1.Areas.Schemas.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api.v1.Areas.Schemas.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrderDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
