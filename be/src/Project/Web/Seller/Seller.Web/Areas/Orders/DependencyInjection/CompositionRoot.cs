using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Orders.ModelBuilders;
using Seller.Web.Areas.Orders.ViewModel;
using Foundation.PageContent.ComponentModels;

namespace Seller.Web.Areas.Orders.DependencyInjection
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
