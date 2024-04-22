using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ExtensionMethods;
using System.Collections.Generic;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Areas.Products.Repositories;
using System.Linq;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Buyer.Web.Areas.Products.ModelBuilders.SearchProducts
{
    public class SearchProductsCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> _searchProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IProductsService _productsService;
        private readonly IOutletRepository _outletRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly LinkGenerator _linkGenerator;

        public SearchProductsCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IProductsService productsService,
            IOutletRepository outletRepository,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator)
        {
            _searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            _productsService = productsService;
            _sidebarModelBuilder = sidebarModelBuilder;
            _modalModelBuilder = modalModelBuilder;
            _outletRepository = outletRepository;
            _inventoryRepository = inventoryRepository;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<SearchProductsCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = _searchProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.Title = componentModel.SearchTerm;
            viewModel.Sidebar = await _sidebarModelBuilder.BuildModelAsync(componentModel);
            viewModel.Modal = await _modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = _globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.ItemsPerPage = ProductConstants.ProductsCatalogPaginationPageSize;
            viewModel.SearchTerm = componentModel.SearchTerm;
            viewModel.ProductsApiUrl = _linkGenerator.GetPathByAction("Get", "SearchProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });

            var products = await _productsService.GetProductsAsync(
                null,
                null,
                null,
                componentModel.Language,
                componentModel.SearchTerm,
                true,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            if (products.Data is not null)
            {
                var outletItems = await _outletRepository.GetOutletProductsByIdsAsync(componentModel.Token, componentModel.Language, products.Data.Select(x => x.Id));
                var inventoryItems = await _inventoryRepository.GetAvailbleProductsInventoryByIds(componentModel.Token, componentModel.Language, products.Data.Select(x => x.Id));

                foreach (var product in products.Data.OrEmptyIfNull())
                {
                    var outletItem = outletItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (outletItem is not null)
                    {
                        product.InOutlet = true;
                        product.AvailableOutletQuantity = outletItem.AvailableQuantity;
                        product.OutletTitle = outletItem.Title;
                    }

                    var inventoryItem = inventoryItems.FirstOrDefault(x => x.ProductSku == product.Sku);

                    if (inventoryItem is not null)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventoryItem.AvailableQuantity;
                        product.ExpectedDelivery = inventoryItem.ExpectedDelivery;
                    }

                    product.CanOrder = true;
                }

                viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, products.PageSize)
                {
                    Data = products.Data
                };
            }

            return viewModel;
        }
    }
}
