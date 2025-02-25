using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Collections.Generic;
using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Areas.Products.Repositories;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly IInventoryRepository inventoryRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOutletRepository outletRepository;

        public AvailableProductsCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator,
            IOutletRepository outletRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.availableProductsCatalogModelBuilder = availableProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
            this.linkGenerator = linkGenerator;
            this.modalModelBuilder = modalModelBuilder;
            this.outletRepository = outletRepository;
        }

        public async Task<AvailableProductsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.availableProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.Title = this.globalLocalizer.GetString("AvailableProducts");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.ItemsPerPage = AvailableProductsConstants.Pagination.ItemsPerPage;
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

            var outletItems = await this.outletRepository.GetOutletProductsAsync(
                componentModel.Language, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId), null, null, componentModel.Language,
                    null, false, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        var availableStockQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableStockQuantity > 0)
                        {
                            product.CanOrder = true;
                            product.AvailableQuantity = availableStockQuantity;
                        }

                        var availableOutletQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableOutletQuantity > 0)
                        {
                            product.AvailableOutletQuantity = availableOutletQuantity;
                        }

                        product.InStock = true;
                        product.ExpectedDelivery = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.ExpectedDelivery;
                    }

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, AvailableProductsConstants.Pagination.ItemsPerPage)
                    {
                        Data = products.Data.OrderBy(x => x.Title)
                    };
                }
            }

            return viewModel;
        }
    }
}
