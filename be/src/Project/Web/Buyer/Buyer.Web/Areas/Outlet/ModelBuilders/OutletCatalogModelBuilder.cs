using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
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
using System.Globalization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Areas.Outlet.ViewModels;
using Buyer.Web.Areas.Outlet.Repositories;
using Buyer.Web.Areas.Outlet.Definitions;

namespace Buyer.Web.Areas.Outlet.ModelBuilders
{
    public class OutletCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OutletPageCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, OutletPageCatalogViewModel> outletCatalogModelBuilder;
        private readonly IProductsService productsService;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> settings;
        private readonly IOutletRepository outletRepository;

        public OutletCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, OutletPageCatalogViewModel> outletCatalogModelBuilder,
            IProductsService productsService,
            IOptions<AppSettings> settings,
            IBasketRepository basketRepository,
            IOutletRepository outletRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.outletCatalogModelBuilder = outletCatalogModelBuilder;
            this.productsService = productsService;
            this.linkGenerator = linkGenerator;
            this.settings = settings;
            this.basketRepository = basketRepository;
            this.outletRepository = outletRepository;
        }

        public async Task<OutletPageCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.outletCatalogModelBuilder.BuildModel(componentModel);

            if (this.settings.Value.IsMarketplace)
            {
                viewModel.ShowBrand = true;
            }

            viewModel.HideQuantityInput = true;
            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.Title = this.globalLocalizer.GetString("Outlet");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.ItemsPerPage = AvailableProductsConstants.Pagination.ItemsPerPage;
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);

            if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
            {
                var existingBasket = await this.basketRepository.GetBasketById(componentModel.Token, componentModel.Language, componentModel.BasketId);

                if (existingBasket != null)
                {
                    var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                    if (productIds.OrEmptyIfNull().Any())
                    {
                        var basketResponseModel = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                            Name = x.ProductName,
                            Sku = x.ProductSku,
                            Quantity = x.Quantity,
                            ExternalReference = x.ExternalReference,
                            ImageSrc = x.PictureUrl,
                            ImageAlt = x.ProductName,
                            DeliveryFrom = x.DeliveryFrom,
                            DeliveryTo = x.DeliveryTo,
                            MoreInfo = x.MoreInfo
                        });

                        viewModel.OrderItems = basketResponseModel;
                    }
                }
            }

            var outletItems = await this.outletRepository.GetOutletProductsAsync(
                componentModel.Language, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

            if (outletItems?.Data is not null && outletItems.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    outletItems.Data.Select(x => x.ProductId.Value), null, null, componentModel.Language, null, PaginationConstants.DefaultPageIndex, OutletConstants.Catalog.DefaultItemsPerPage, componentModel.Token);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = outletItems.Data.FirstOrDefault(x => x.ProductId == product.Id)?.Quantity;
                    }
                }
                    
                viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, OutletConstants.Catalog.DefaultItemsPerPage)
                {
                    Data = products.Data.OrderBy(x => x.Title)
                };
            }

            return viewModel;
        }
    }
}
