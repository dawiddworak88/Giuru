using Buyer.Web.Areas.Orders.ComponentModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.ModelBuilders;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Repositories.ClientNotificationTypeApproval;
using Buyer.Web.Areas.Orders.Repositories.NotificationTypeApproval;
using Buyer.Web.Areas.Orders.Services.OrderFiles;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderFileService, OrderFileService>();
            services.AddScoped<IClientNotificationTypeApproval, ClientNotoficationTypeApprovalRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, CatalogOrderViewModel<Order>>, OrdersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel>, OrdersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>, OrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderPageViewModel>, StatusOrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrdersPageComponentModel, StatusOrderFormViewModel>, StatusOrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel>, OrderItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>, OrderItemFormModelBuilder>();
        }
    }
}
