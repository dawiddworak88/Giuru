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
using Seller.Web.Areas.Inventory.Repositories.Warehouses;

namespace Seller.Web.Areas.Inventory.ModelBuilders
{
    public class WarehousesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Warehouse>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IWarehousesRepository warehousesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer warehouseLocalizer;
        private readonly LinkGenerator linkGenerator;

        public WarehousesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IWarehousesRepository warehousesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> warehouseLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.warehousesRepository = warehousesRepository;
            this.globalLocalizer = globalLocalizer;
            this.warehouseLocalizer = warehouseLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Warehouse>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<Warehouse>, Warehouse>();

            viewModel.Title = this.warehouseLocalizer.GetString("Warehouses");
            viewModel.NewText = this.warehouseLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name});

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Warehouse.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[] 
                { 
                    this.globalLocalizer.GetString("Name"),
                    this.globalLocalizer.GetString("Location"),
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
                        Title = nameof(Warehouse.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Warehouse.Location).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Warehouse.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Warehouse.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.warehousesRepository.GetWarehousesAsync(componentModel.Token, componentModel.Language, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage, $"{nameof(Warehouse.CreatedDate)} desc");

            return viewModel;
        }
    }
}