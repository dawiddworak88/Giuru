using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.Repositories;
using Foundation.GenericRepository.Definitions;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class OutletPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer inventoryLocalizer;
        private readonly IOutletRepository outletRepository;

        public OutletPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            LinkGenerator linkGenerator,
            IOutletRepository outletRepository)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.inventoryLocalizer = inventoryLocalizer;
            this.outletRepository = outletRepository;
        }

        public async Task<CatalogViewModel<OutletItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<OutletItem>, OutletItem>();

            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.NewText = this.inventoryLocalizer.GetString("NewOutletText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Outlet", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Outlet", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "OutletsApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "OutletsApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(OutletItem.CreatedDate)} desc";

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
                        Title = nameof(OutletItem.WarehouseName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.ProductName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.ProductSku).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.Quantity).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.AvailableQuantity).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(OutletItem.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.outletRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(OutletItem.CreatedDate)} desc");

            return viewModel;
        }
    }
}