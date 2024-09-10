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
using Buyer.Web.Areas.Products.ComponentModels;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsCatalogModelBuilder : IAsyncComponentModelBuilder<ProductsComponentModel, AvailableProductsCatalogViewModel>
    {
        private readonly IStringLocalizer _globalLocalizer;
        private readonly ICatalogModelBuilder<ProductsComponentModel, AvailableProductsCatalogViewModel> _availableProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IProductsService _productsService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly LinkGenerator _linkGenerator;

        public AvailableProductsCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _availableProductsCatalogModelBuilder = availableProductsCatalogModelBuilder;
            _productsService = productsService;
            _inventoryRepository = inventoryRepository;
            _linkGenerator = linkGenerator;
            _modalModelBuilder = modalModelBuilder;
        }

        public async Task<AvailableProductsCatalogViewModel> BuildModelAsync(ProductsComponentModel componentModel)
        {
            var viewModel = _availableProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = _globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.Title = _globalLocalizer.GetString("AvailableProducts");
            viewModel.ProductsApiUrl = _linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.ItemsPerPage = AvailableProductsConstants.Pagination.ItemsPerPage;
            viewModel.Modal = await _modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);

            var inventories = await _inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await _productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId), null, componentModel.SellerId, componentModel.UserEmail, componentModel.Language,
                    null, false, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        var availableQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableQuantity > 0)
                        {
                            product.CanOrder = true;
                            product.AvailableQuantity = availableQuantity;
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
