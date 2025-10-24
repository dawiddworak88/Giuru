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
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.DomainModels.Prices;
using System;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Areas.Products.ComponentModels;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Shared.ViewModels.Filters;
using Foundation.GenericRepository.Definitions;

namespace Buyer.Web.Areas.Products.ModelBuilders
{
    public class OutletCatalogModelBuilder : IAsyncComponentModelBuilder<PriceComponentModel, OutletPageCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<PriceComponentModel, OutletPageCatalogViewModel> outletCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly LinkGenerator linkGenerator;
        private readonly IOutletRepository outletRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public OutletCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, OutletPageCatalogViewModel> outletCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            IOutletRepository outletRepository,
            LinkGenerator linkGenerator,
            IInventoryRepository inventoryRepository,
            IOptions<AppSettings> options,
            IPriceService priceService,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.outletCatalogModelBuilder = outletCatalogModelBuilder;
            this.productsService = productsService;
            this.linkGenerator = linkGenerator;
            this.outletRepository = outletRepository;
            this.modalModelBuilder = modalModelBuilder;
            this.inventoryRepository = inventoryRepository;
            _options = options;
            _priceService = priceService;
            _productLocalizer = productLocalizer;
        }

        public async Task<OutletPageCatalogViewModel> BuildModelAsync(PriceComponentModel componentModel)
        {
            var viewModel = this.outletCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "OutletApi", new { Area = "Products" });
            viewModel.ItemsPerPage = Constants.DefaultItemsPerPage;
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, Constants.DefaultItemsPerPage);
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.IsDefaultOutletOrder = true;
            viewModel.Filters = componentModel.Filters;

            var products = await this.productsService.GetProductsAsync(
                componentModel.Token,
                componentModel.Language,
                null,
                Constants.DefaultPageIndex,
                Constants.DefaultItemsPerPage,
                "outlet",
                SortingConstants.Default,
                componentModel.Filters);

            if (products.Data.OrEmptyIfNull().Any())
            {
                var outletItems = await this.outletRepository.GetOutletProductsByProductsIdAsync(
                    componentModel.Token,
                    componentModel.Language,
                    products.Data.Select(x => x.Id));

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
                            ShelfType = x.ShelfType,
                            IsOutlet = (outletItems.FirstOrDefault(y => y.ProductId == x.Id)?.AvailableQuantity > 0).ToYesOrNo()
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

                    var availableOutletQuantity = outletItems.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                    if (availableOutletQuantity > 0)
                    {
                        product.AvailableOutletQuantity = availableOutletQuantity;
                        product.CanOrder = true;
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
                            Label = this.globalLocalizer.GetString("Dimensions"),
                            IsNested = true,
                            Items = new List<NestedFilterItemViewModel>
                            {
                                new NestedFilterItemViewModel
                                {
                                    Label = this.globalLocalizer.GetString("Height"),
                                    Key = "height",
                                    Items = products.Filters.FirstOrDefault(x => x.Name == "height")?.Values.Select(x => new FilterItemViewModel
                                    {
                                        Label = x,
                                        Value = x
                                    })
                                },
                                new NestedFilterItemViewModel
                                {
                                    Label = this.globalLocalizer.GetString("Width"),
                                    Key = "width",
                                    Items = products.Filters.FirstOrDefault(x => x.Name == "width")?.Values.Select(x => new FilterItemViewModel
                                    {
                                        Label = x,
                                        Value = x
                                    })
                                },
                                new NestedFilterItemViewModel
                                {
                                    Label = this.globalLocalizer.GetString("Depth"),
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

                viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, products.PageSize)
                {
                    Data = products.Data
                };
            }

            return viewModel;
        }
    }
}