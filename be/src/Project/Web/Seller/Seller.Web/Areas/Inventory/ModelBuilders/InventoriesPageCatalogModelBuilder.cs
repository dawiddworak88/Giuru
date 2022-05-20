using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class InventoriesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<InventoryItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer inventoryLocalizer;
        private readonly LinkGenerator linkGenerator;

        public InventoriesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IInventoryRepository inventoryRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.inventoryRepository = inventoryRepository;
            this.globalLocalizer = globalLocalizer;
            this.inventoryLocalizer = inventoryLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<InventoryItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<InventoryItem>, InventoryItem>();

            viewModel.Title = this.inventoryLocalizer.GetString("Inventory");
            viewModel.NewText = this.inventoryLocalizer.GetString("NewTextInventory");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Inventory", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "InventoriesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Inventory", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "InventoriesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.OrderBy = $"{nameof(InventoryItem.Sku)} ASC";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.inventoryLocalizer.GetString("Warehouse"),
                    this.inventoryLocalizer.GetString("SelectProductLabel"),
                    this.inventoryLocalizer.GetString("Sku"),
                    this.inventoryLocalizer.GetString("QuantityLabel"),
                    this.inventoryLocalizer.GetString("AvailableQuantityLabel"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.WarehouseName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.ProductName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.Sku).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.Quantity).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.AvailableQuantity).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(InventoryItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.inventoryRepository.GetInventoryProductsAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(InventoryItem.Sku)} ASC"); ;

            return viewModel;
        }
    }
}
