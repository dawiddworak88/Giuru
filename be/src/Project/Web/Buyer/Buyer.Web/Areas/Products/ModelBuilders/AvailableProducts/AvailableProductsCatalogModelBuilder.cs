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
using System.Globalization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.Extensions.ExtensionMethods;
using Buyer.Web.Areas.Orders.ApiResponseModels;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder;
        private readonly IProductsService productsService;
        private readonly IBasketRepository basketRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> settings;

        public AvailableProductsCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder,
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            IOptions<AppSettings> settings,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.availableProductsCatalogModelBuilder = availableProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
            this.linkGenerator = linkGenerator;
            this.settings = settings;
            this.basketRepository = basketRepository;
        }

        public async Task<AvailableProductsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.availableProductsCatalogModelBuilder.BuildModel(componentModel);

            if (this.settings.Value.IsMarketplace)
            {
                viewModel.ShowBrand = true;
            }

            viewModel.ShowAddToCartButton = true;
            viewModel.SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct");
            viewModel.UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.Title = this.globalLocalizer.GetString("AvailableProducts");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);

            if (viewModel.IsLoggedIn)
            {
                var existingBasket = await this.basketRepository.GetBasketByOrganisation(componentModel.Token, componentModel.Language);
                if (existingBasket != null)
                {
                    viewModel.Id = existingBasket.Id.Value;
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

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId), null, null, componentModel.Language,
                    null, PaginationConstants.DefaultPageIndex, ProductConstants.ProductsCatalogPaginationPageSize, componentModel.Token);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;
                    }

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, ProductConstants.ProductsCatalogPaginationPageSize)
                    {
                        Data = products.Data.OrderByDescending(x => x.AvailableQuantity)
                    };
                }
            }

            return viewModel;
        }
    }
}
