using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.ModelBuilders;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Inventory.Repositories.Warehouses;
using Seller.Web.Areas.Inventory.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Inventory.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterInventoryAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IWarehousesRepository, WarehousesRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Warehouse>>, WarehousesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, WarehousesPageViewModel>, WarehousesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditWarehousePageViewModel>, EditWarehousePageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditWarehouseFormViewModel>, EditWarehouseFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<InventoryItem>>, InventoriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, InventoriesPageViewModel>, InventoriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryPageViewModel>, EditInventoryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditInventoryFormViewModel>, EditInventoryFormModelBuilder>();
        }
    }
}
