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
using Seller.Web.Areas.Orders.ComponentModels;

namespace Seller.Web.Areas.Orders.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOrdersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderFileService, OrderFileService>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Order>>, OrdersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrdersPageViewModel>, OrdersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel>, OrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>, OrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditOrderPageViewModel>, EditOrderPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditOrderFormViewModel>, EditOrderFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel>, OrderItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>, OrderItemFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributesPageViewModel>, OrderAttributesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributePageViewModel>, OrderAttributePageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributeFormViewModel>, OrderAttributeFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttribute>>, OrderAttributesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OrderAttributeOption>>, OrderAttributePageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionPageViewModel>, OrderAttributeOptionPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionFormViewModel>, OrderAttributeOptionFormModelBuilder>();
        }
    }
}