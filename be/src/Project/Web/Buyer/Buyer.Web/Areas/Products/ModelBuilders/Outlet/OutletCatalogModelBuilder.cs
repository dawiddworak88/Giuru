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
using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Shared.Repositories.LeadTime;
using Buyer.Web.Shared.Services.DeliveryDates;

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
        private readonly ILeadTimeRepository _leadTimeRepository;
        private readonly IDeliveryMessageHelper _deliveryMessageHelper;
        private readonly IExpectedDeliveryDateService _expectedDeliveryDateService;

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
            ILeadTimeRepository leadTimeRepository,
            IDeliveryMessageHelper deliveryMessageHelper,
            IExpectedDeliveryDateService expectedDeliveryDateService)
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
            _leadTimeRepository = leadTimeRepository;
            _deliveryMessageHelper = deliveryMessageHelper;
            _expectedDeliveryDateService = expectedDeliveryDateService;
        }

        public async Task<OutletPageCatalogViewModel> BuildModelAsync(PriceComponentModel componentModel)
        {
            var viewModel = this.outletCatalogModelBuilder.BuildModel(componentModel);

            viewModel.ShowAddToCartButton = true;
            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "OutletApi", new { Area = "Products" });
            viewModel.ItemsPerPage = OutletConstants.Catalog.DefaultItemsPerPage;
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.IsDefaultOutletOrder = true;

            var outletItems = await this.outletRepository.GetOutletProductsAsync(
                componentModel.Language, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language, PaginationConstants.DefaultPageIndex, AvailableProductsConstants.Pagination.ItemsPerPage, componentModel.Token);

            if (outletItems?.Data is not null && outletItems.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    outletItems.Data.Select(x => x.ProductId), null, null, componentModel.Language, null, false, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

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
                                ShelfType = x.ShelfType,
                                IsOutlet = (outletItems.Data.FirstOrDefault(y => y.ProductId == x.Id)?.AvailableQuantity > 0).ToYesOrNo()
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

                    var leadTimes = await _leadTimeRepository.GetLeadTimesAsync(
                        accessToken: componentModel.Token,
                        skus: [.. products.Data.Select(x => x.Sku)]);

                    for (int i = 0; i < products.Data.Count(); i++)
                    {
                        var product = products.Data.ElementAtOrDefault(i);

                        if (product is null)
                        {
                            continue;
                        }

                        var availableOutletQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableOutletQuantity > 0)
                        {
                            product.AvailableOutletQuantity = availableOutletQuantity;
                            product.CanOrder = true;
                            product.InOutlet = true;
                        }

                        product.OutletTitle = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.Title;
                        product.OutletDescription = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.Description;

                        var availableStockQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;

                        if (availableStockQuantity > 0)
                        {
                            product.AvailableQuantity = availableStockQuantity;
                            product.InStock = true;
                        }

                        var price = prices.ElementAtOrDefault(i);

                        if (price is not null)
                        {
                            product.Price = new ProductPriceViewModel
                            {
                                Currency = price.CurrencyCode,
                                Current = price.CurrentPrice
                            };
                        }

                        var leadTimeDays = leadTimes?.Items?.FirstOrDefault(x => x.Sku == product.Sku)?.LeadTimeDays ?? 0;
                        product.ExpectedLeadTime = leadTimeDays > 0
                            ? DateOnly.FromDateTime(_expectedDeliveryDateService.CalculateExpectedDeliveryDate(leadTimeDays))
                            : null;
                        product.LeadTimeDeliveryMessage = _deliveryMessageHelper.GetDeliveryMessage(componentModel.DeliveryType, product.InStock);
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
