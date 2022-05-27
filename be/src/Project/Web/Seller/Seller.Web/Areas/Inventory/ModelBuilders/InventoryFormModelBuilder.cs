using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Inventory.Repositories.Warehouses;
using Seller.Web.Areas.Inventory.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Products;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class InventoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, InventoryFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> inventoryLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IInventoryRepository inventoriesRepository;
        private readonly IWarehousesRepository warehousesRepository;
        private readonly IProductsRepository productsRepository;

        public InventoryFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            LinkGenerator linkGenerator,
            IProductsRepository productsRepository,
            IWarehousesRepository warehousesRepository,
            IInventoryRepository inventoriesRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.inventoryLocalizer = inventoryLocalizer;
            this.linkGenerator = linkGenerator;
            this.warehousesRepository = warehousesRepository;
            this.productsRepository = productsRepository;
            this.inventoriesRepository = inventoriesRepository;
        }

        public async Task<InventoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new InventoryFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.inventoryLocalizer.GetString("EditInventory"),
                SelectWarehouseLabel = this.inventoryLocalizer.GetString("Warehouse"),
                SelectProductLabel = this.inventoryLocalizer.GetString("SelectProductLabel"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                CancelLabel = this.globalLocalizer.GetString("Cancel"),
                SaveText = this.inventoryLocalizer.GetString("SaveText"),
                WarehouseRequiredErrorMessage = this.inventoryLocalizer.GetString("WarehouseRequiredErrorMessage"),
                ProductRequiredErrorMessage = this.inventoryLocalizer.GetString("ProductRequiredErrorMessage"),
                NavigateToInventoryListText = this.inventoryLocalizer.GetString("NavigateToInventoryListText"),
                QuantityLabel = this.inventoryLocalizer.GetString("QuantityLabel"),
                QuantityRequiredErrorMessage = this.inventoryLocalizer.GetString("QuantityRequiredErrorMessage"),
                QuantityFormatErrorMessage = this.inventoryLocalizer.GetString("QuantityFormatErrorMessage"),
                RestockableInDaysLabel = this.inventoryLocalizer.GetString("RestockableInDaysLabel"),
                AvailableQuantityLabel = this.inventoryLocalizer.GetString("AvailableQuantityLabel"),
                ExpectedDeliveryLabel = this.inventoryLocalizer.GetString("ExpectedDeliveryLabel"),
                SelectWarehouse = this.inventoryLocalizer.GetString("SelectWarehouse"),
                ChangeExpectedDeliveryLabel = this.inventoryLocalizer.GetString("ChangeExpectedDeliveryLabel"),
                InventoryUrl = this.linkGenerator.GetPathByAction("Index", "Inventories", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "InventoriesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name }),
                EanLabel = this.globalLocalizer.GetString("Ean"),
                ProductsSuggestionUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            var warehouses = await this.warehousesRepository.GetAllWarehousesAsync(componentModel.Token, componentModel.Language, null);
            if (warehouses != null)
            {
                viewModel.Warehouses = warehouses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var products = await this.productsRepository.GetAllProductsAsync(componentModel.Token, componentModel.Language, null);
            if (products != null)
            {
                viewModel.Products = products.Select(x => new ListInventoryItemViewModel { Id = x.Id, Name = x.Name, Sku = x.Sku });
            }

            if (componentModel.Id.HasValue)
            {
                var inventoryProduct = await this.inventoriesRepository.GetInventoryProductAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                if (inventoryProduct != null)
                {
                    viewModel.Id = inventoryProduct.Id;
                    viewModel.WarehouseId = inventoryProduct.WarehouseId;
                    viewModel.ProductId = inventoryProduct.ProductId;
                    viewModel.Quantity = inventoryProduct.Quantity;
                    viewModel.Ean = inventoryProduct.Ean;
                    viewModel.AvailableQuantity = inventoryProduct.AvailableQuantity;
                    viewModel.RestockableInDays = inventoryProduct.RestockableInDays;
                    viewModel.ExpectedDelivery = inventoryProduct.ExpectedDelivery;
                }
            }

            return viewModel;
        }
    }
}
