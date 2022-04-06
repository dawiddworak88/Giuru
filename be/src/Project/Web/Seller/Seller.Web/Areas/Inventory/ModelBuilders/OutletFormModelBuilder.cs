using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Inventory.Repositories.Warehouses;
using Seller.Web.Areas.Inventory.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Products;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class OutletFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OutletFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> inventoryLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IWarehousesRepository warehousesRepository;
        private readonly IProductsRepository productsRepository;
        private readonly IOutletRepository outletRepository;

        public OutletFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            LinkGenerator linkGenerator,
            IWarehousesRepository warehousesRepository,
            IOutletRepository outletRepository,
            IProductsRepository productsRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.inventoryLocalizer = inventoryLocalizer;
            this.linkGenerator = linkGenerator;
            this.warehousesRepository = warehousesRepository;
            this.outletRepository = outletRepository;
            this.productsRepository = productsRepository;
        }

        public async Task<OutletFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OutletFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.inventoryLocalizer.GetString("OutletItem"),
                SelectWarehouseLabel = this.inventoryLocalizer.GetString("Warehouse"),
                SelectProductLabel = this.inventoryLocalizer.GetString("SelectProductLabel"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                CancelLabel = this.globalLocalizer.GetString("Cancel"),
                SaveText = this.inventoryLocalizer.GetString("SaveText"),
                WarehouseRequiredErrorMessage = this.inventoryLocalizer.GetString("WarehouseRequiredErrorMessage"),
                ProductRequiredErrorMessage = this.inventoryLocalizer.GetString("ProductRequiredErrorMessage"),
                NavigateToOutletListText = this.inventoryLocalizer.GetString("NavigateToOutletListText"),
                QuantityLabel = this.inventoryLocalizer.GetString("QuantityLabel"),
                QuantityRequiredErrorMessage = this.inventoryLocalizer.GetString("QuantityRequiredErrorMessage"),
                QuantityFormatErrorMessage = this.inventoryLocalizer.GetString("QuantityFormatErrorMessage"),
                AvailableQuantityLabel = this.inventoryLocalizer.GetString("AvailableQuantityLabel"),
                SelectWarehouse = this.inventoryLocalizer.GetString("SelectWarehouse"),
                OutletUrl = this.linkGenerator.GetPathByAction("Index", "Outlets", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "OutletsApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name })
            };

            var warehouses = await this.warehousesRepository.GetAllWarehousesAsync(componentModel.Token, componentModel.Language, null);
            if (warehouses != null)
            {
                viewModel.Warehouses = warehouses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var products = await this.productsRepository.GetAllProductsAsync(componentModel.Token, componentModel.Language, null);

            if (products is not null)
            {
                viewModel.Products = products.Select(x => new ListOutletItemViewModel { Id = x.Id, Name = x.Name, Sku = x.Sku });
            }

            if (componentModel.Id.HasValue)
            {
                var outletItem = await this.outletRepository.GetOutletItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                
                if (outletItem is not null)
                {
                    viewModel.Id = outletItem.Id;
                    viewModel.WarehouseId = outletItem.WarehouseId;
                    viewModel.ProductId = outletItem.ProductId;
                    viewModel.Quantity = outletItem.Quantity;
                    viewModel.AvailableQuantity = outletItem.AvailableQuantity;
                }
            }

            return viewModel;
        }
    }
}
