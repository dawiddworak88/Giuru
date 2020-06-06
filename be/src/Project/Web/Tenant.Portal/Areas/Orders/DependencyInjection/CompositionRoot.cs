using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Orders.ModelBuilders;
using Tenant.Portal.Areas.Orders.ViewModel;

namespace Tenant.Portal.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<OrderPageViewModel>, OrderPageModelBuilder>();
        }
    }
}
