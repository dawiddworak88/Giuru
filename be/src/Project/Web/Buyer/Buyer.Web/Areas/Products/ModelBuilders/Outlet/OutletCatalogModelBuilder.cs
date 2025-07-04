using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Collections.Generic;
using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Areas.Products.ViewModels;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Shared.Definitions.Filters;

namespace Buyer.Web.Areas.Products.ModelBuilders
{
    public class OutletCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OutletPageCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, OutletPageCatalogViewModel> outletCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly LinkGenerator linkGenerator;
        private readonly IOutletRepository outletRepository;
        private readonly IInventoryRepository inventoryRepository;

        public OutletCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, OutletPageCatalogViewModel> outletCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            IOutletRepository outletRepository,
            LinkGenerator linkGenerator,
            IInventoryRepository inventoryRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.outletCatalogModelBuilder = outletCatalogModelBuilder;
            this.productsService = productsService;
            this.linkGenerator = linkGenerator;
            this.outletRepository = outletRepository;
            this.modalModelBuilder = modalModelBuilder;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<OutletPageCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.outletCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "OutletApi", new { Area = "Products" });
            viewModel.ItemsPerPage = OutletConstants.Catalog.DefaultItemsPerPage;
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);

            var outletItems = await this.outletRepository.GetOutletProductsAsync(
                componentModel.Language, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

            if (outletItems?.Data is not null && outletItems.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    outletItems.Data.Select(x => x.ProductId), null, null, componentModel.Language, null, false, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token, SortingConstants.Default);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        var availableOutletQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableOutletQuantity > 0)
                        {
                            product.CanOrder = true;
                            product.AvailableOutletQuantity = availableOutletQuantity;
                        }

                        var availableStockQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableStockQuantity > 0)
                        {
                            product.AvailableQuantity = availableStockQuantity;
                        }

                        product.InOutlet = true;
                        product.OutletTitle = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.Title;
                        product.OutletDescription = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.Description;
                    }
                }

                viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(outletItems.Total, OutletConstants.Catalog.DefaultItemsPerPage)
                {
                    Data = products.Data.OrderBy(x => x.Title)
                };
            }

            return viewModel;
        }
    }
}
