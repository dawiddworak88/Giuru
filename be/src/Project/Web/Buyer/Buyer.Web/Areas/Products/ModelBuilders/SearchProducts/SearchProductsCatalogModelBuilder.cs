using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Filters;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Foundation.GenericRepository.Definitions;

namespace Buyer.Web.Areas.Products.ModelBuilders.SearchProducts
{
    public class SearchProductsCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> _searchProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IProductsService _productsService;
        private readonly IOutletRepository _outletRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;

        public SearchProductsCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IProductsService productsService,
            IOutletRepository outletRepository,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator,
            IStringLocalizer<ProductResources> productLocalizer,
            IOptions<AppSettings> options,
            IPriceService priceService
        )
        {
            _searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            _productsService = productsService;
            _sidebarModelBuilder = sidebarModelBuilder;
            _modalModelBuilder = modalModelBuilder;
            _outletRepository = outletRepository;
            _inventoryRepository = inventoryRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _linkGenerator = linkGenerator;
            _options = options;
            _priceService = priceService;
        }

        public async Task<SearchProductsCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = _searchProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.Title = componentModel.SearchTerm;
            viewModel.Sidebar = await _sidebarModelBuilder.BuildModelAsync(componentModel);
            viewModel.Modal = await _modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.ShowAddToCartButton = true;
            viewModel.ItemsPerPage = ProductConstants.ProductsCatalogPaginationPageSize;
            viewModel.SearchTerm = componentModel.SearchTerm;
            viewModel.ProductsApiUrl = _linkGenerator.GetPathByAction("Get", "SearchProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.FilterCollector = new FiltersCollectorViewModel
            {
                AllFilters = _productLocalizer.GetString("AllFilters"),
                SortLabel = _productLocalizer.GetString("SortLabel"),
                ClearAllFilters = _productLocalizer.GetString("ClearAllFilters"),
                SeeResult = _productLocalizer.GetString("SeeResult"),
                FiltersLabel = _productLocalizer.GetString("FiltersLabel"),
                SortItems = new List<SortItemViewModel>
                    {
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortDefault"), Key = SortingConstants.Default },
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortNewest"), Key = SortingConstants.Newest },
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortName"), Key = SortingConstants.Name }
                    }
            };

            var products = await _productsService.GetProductsAsync(
                null,
                null,
                null,
                componentModel.Language,
                componentModel.SearchTerm,
                true,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token,
                SortingConstants.Default);

            if (products.Data is not null)
            {
                var outletItems = await _outletRepository.GetOutletProductsByIdsAsync(componentModel.Token, componentModel.Language, products.Data.Select(x => x.Id));
                var inventoryItems = await _inventoryRepository.GetAvailbleProductsInventoryByIds(componentModel.Token, componentModel.Language, products.Data.Select(x => x.Id));

                var prices = Enumerable.Empty<Price>();

                if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    prices = await _priceService.GetPrices(
                        _options.Value.GrulaAccessToken,
                        DateTime.UtcNow,
                        products.Data.Select(x => new PriceProduct
                        {
                            PrimarySku = x.PrimaryProductSku,
                            FabricsGroup = x.FabricsGroup,
                            SleepAreaSize = x.SleepAreaSize,
                            ExtraPacking = x.ExtraPacking,
                            PaletteSize = x.PaletteSize,
                            Size = x.Size,
                            PointsOfLight = x.PointsOfLight,
                            LampshadeType = x.LampshadeType,
                            LampshadeSize = x.LampshadeSize,
                            LinearLight = x.LinearLight,
                            Mirror = x.Mirror,
                            Shape = x.Shape,
                            PrimaryColor = x.PrimaryColor,
                            SecondaryColor = x.SecondaryColor,
                            ShelfType = x.ShelfType
                        }),
                        new PriceClient
                        {
                            Id = componentModel.ClientId,
                            Name = componentModel.Name,
                            CurrencyCode = componentModel.CurrencyCode,
                            ExtraPacking = componentModel.ExtraPacking,
                            PaletteLoading = componentModel.PaletteLoading,
                            Country = componentModel.Country,
                            DeliveryZipCode = componentModel.DeliveryZipCode
                        });
                }

                for (int i = 0; i < products.Data.Count(); i++)
                {
                    var product = products.Data.ElementAtOrDefault(i);

                    if (product is null)
                    {
                        continue;
                    }

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

                    if (prices.Any())
                    {
                        var price = prices.ElementAtOrDefault(i);

                        if (price is not null)
                        {
                            product.Price = new ProductPriceViewModel
                            {
                                Current = price.CurrentPrice,
                                Currency = price.CurrencyCode
                            };
                        }
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
