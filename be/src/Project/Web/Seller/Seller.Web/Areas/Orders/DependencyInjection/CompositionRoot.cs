using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Orders.ModelBuilders;
using Seller.Web.Areas.Orders.ViewModel;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.Repositories.Baskets;

namespace Seller.Web.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IBasketsRepository, BasketsRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Order>>, OrdersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrdersPageViewModel>, OrdersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>, OrderFormModelBuilder>();
        }
    }
}
