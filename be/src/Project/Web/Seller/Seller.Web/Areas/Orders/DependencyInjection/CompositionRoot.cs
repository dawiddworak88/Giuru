using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Orders.ViewModel;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Services.OrderFiles;
using Seller.Web.Areas.Orders.ModelBuilders;
using Seller.Web.Areas.Orders.Repositories.ClientNotificationTypeApproval;
using Seller.Web.Areas.Orders.ComponetModels;

namespace Seller.Web.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderFileService, OrderFileService>();
            services.AddScoped<IClientNotificationTypeApprovalRepository, ClientNotificationTypeApprovalRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogViewModel<Order>>, OrdersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel>, OrdersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>, OrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderPageViewModel>, EditOrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, EditOrderFormViewModel>, EditOrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel>, OrderItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>, OrderItemFormModelBuilder>();
        }
    }
}