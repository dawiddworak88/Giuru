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
using Buyer.Web.Shared.DomainModels.Prices;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.Prices;
using System;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Shared.ViewModels.Filters;
using Foundation.GenericRepository.Definitions;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsCatalogModelBuilder : IAsyncComponentModelBuilder<PriceComponentModel, AvailableProductsCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly IInventoryRepository inventoryRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOutletRepository outletRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public AvailableProductsCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator,
            IOutletRepository outletRepository,
            IOptions<AppSettings> options,
            IPriceService priceService,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.availableProductsCatalogModelBuilder = availableProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
            this.linkGenerator = linkGenerator;
            this.modalModelBuilder = modalModelBuilder;
            this.outletRepository = outletRepository;
            _options = options;
            _priceService = priceService;
            _productLocalizer = productLocalizer;
        }

        public async Task<AvailableProductsCatalogViewModel> BuildModelAsync(PriceComponentModel componentModel)
        {
            var viewModel = this.availableProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.Title = this.globalLocalizer.GetString("AvailableProducts");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.ItemsPerPage = AvailableProductsConstants.Pagination.ItemsPerPage;
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);
            viewModel.Filters = componentModel.Filters;

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language, 
                PaginationConstants.DefaultPageIndex, 
                AvailableProductsConstants.Pagination.ItemsPerPage, 
                componentModel.Token);

            var outletItems = await this.outletRepository.GetOutletProductsAsync(
                componentModel.Language, 
                PaginationConstants.DefaultPageIndex, 
                OutletConstants.Catalog.DefaultItemsPerPage, 
                componentModel.Token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    componentModel.Token,
                    componentModel.Language,
                    inventories.Data.Select(x => x.ProductId),
                    componentModel.Filters,
                    null,
                    PaginationConstants.DefaultPageIndex,
                    AvailableProductsConstants.Pagination.ItemsPerPage,
                    SortingConstants.Default);

                if (products is not null)
                {
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

                        var availableStockQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableStockQuantity > 0)
                        {
                            product.AvailableQuantity = availableStockQuantity;
                            product.CanOrder = true;
                            product.InStock = true;
                            product.ExpectedDelivery = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.ExpectedDelivery;
                        }

                        var availableOutletQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableOutletQuantity > 0)
                        {
                            product.AvailableOutletQuantity = availableOutletQuantity;
                            product.InOutlet = true;
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
                    }

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
                        },
                        FilterInputs = new List<FilterViewModel>
                        {
                            new SingleFilterViewModel
                            {
                                Key = "category",
                                Label = this.globalLocalizer.GetString("Category"),
                                Items = products.Filters.FirstOrDefault(x => x.Name == "category").Values.Select(x => new FilterItemViewModel
                                {
                                    Label = x,
                                    Value = x
                                })
                            },
                            new SingleFilterViewModel
                            {
                                Key = "color",
                                Label = this.globalLocalizer.GetString("Color"),
                                Items = products.Filters.FirstOrDefault(x => x.Name == "color").Values.Select(x => new FilterItemViewModel
                                {
                                    Label = x,
                                    Value = x
                                })
                            },
                            new NestedFilterViewModel
                        {
                            Key = "dimensions",
                            Label = "Wymiary",
                            IsNested = true,
                            Items = new List<NestedFilterItemViewModel>
                            {
                                new NestedFilterItemViewModel
                                {
                                    Label = "Wysokość",
                                    Key = "height",
                                    Items = products.Filters.FirstOrDefault(x => x.Name == "height")?.Values.Select(x => new FilterItemViewModel
                                    {
                                        Label = x,
                                        Value = x
                                    })
                                },
                                new NestedFilterItemViewModel
                                {
                                    Label = "Szerokość",
                                    Key = "width",
                                    Items = products.Filters.FirstOrDefault(x => x.Name == "width")?.Values.Select(x => new FilterItemViewModel
                                    {
                                        Label = x,
                                        Value = x
                                    })
                                },
                                new NestedFilterItemViewModel
                                {
                                    Label = "Głębokość",
                                    Key = "depth",
                                    Items = products.Filters.FirstOrDefault(x => x.Name == "depth")?.Values.Select(x => new FilterItemViewModel
                                    {
                                        Label = x,
                                        Value = x
                                    })
                                },
                            }
                        }
                        }
                    };

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, AvailableProductsConstants.Pagination.ItemsPerPage)
                    {
                        Data = products.Data
                    };
                }
            }

            return viewModel;
        }
    }
}
