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
using Seller.Web.Areas.Outlet.DomainModels;

namespace Seller.Web.Areas.Outlet.ModelBuilders
{
    public class OutletPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer outletLocalizer;

        public OutletPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OutletResources> outletLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.outletLocalizer = outletLocalizer;
        }

        public async Task<CatalogViewModel<OutletItem>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<OutletItem>, OutletItem>();

            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.NewText = this.outletLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Warehouse", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "WarehousesApi", new { Area = "Inventory", culture = CultureInfo.CurrentUICulture.Name });

            //viewModel.OrderBy = $"{nameof(OutletItem.CreatedDate)} desc";

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
                    /*new CatalogPropertyViewModel
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
                    }*/
                }
            };

            //viewModel.PagedItems = await this.warehousesRepository.GetWarehousesAsync(componentModel.Token, componentModel.Language, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage, $"{nameof(Warehouse.CreatedDate)} desc");

            return viewModel;
        }
    }
}