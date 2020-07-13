using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Portal.Areas.Orders.ModelBuilders;
using Seller.Portal.Areas.Orders.ViewModel;
using Seller.Portal.Shared.ComponentModels;

namespace Seller.Portal.Areas.Orders.DependencyInjection
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
