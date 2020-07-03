using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Portal.Areas.Orders.ModelBuilders;
using Tenant.Portal.Areas.Orders.ViewModel;
using Tenant.Portal.Shared.ComponentModels;

namespace Tenant.Portal.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IModelBuilder<OrderDetailPageViewModel>, OrderDetailPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderPageViewModel>, ImportOrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderFormViewModel>, ImportOrderFormModelBuilder>();
        }
    }
}
