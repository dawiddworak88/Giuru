using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Buyer.Web.Shared.ViewModels.Images;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.ViewModels.Sidebar;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<InventoryResources> inventoryResources;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderResources;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly ICdnService cdnService;
        private readonly IBasketRepository basketRepository;

        public ProductDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IStringLocalizer<InventoryResources> inventoryResources,
            IStringLocalizer<OrderResources> orderResources,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            ICdnService cdnService)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.options = options;
            this.mediaService = mediaService;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.inventoryResources = inventoryResources;
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
            this.cdnService = cdnService;
            this.orderResources = orderResources;
        }

        public async Task<ProductDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductDetailViewModel
            {
                ByLabel = this.globalLocalizer.GetString("By"),
                DescriptionLabel = this.globalLocalizer.GetString("Description"),
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductInformationLabel = this.productLocalizer.GetString("ProductInformation"),
                PricesLabel = this.globalLocalizer.GetString("Prices"),
                SuccessfullyAddedProduct = this.globalLocalizer.GetString("SuccessfullyAddedProduct"),
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                SignInUrl = "#",
                UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                BasketLabel = this.globalLocalizer.GetString("BasketLabel"),
                SkuLabel = this.productLocalizer.GetString("Sku"),
                InStockLabel = this.globalLocalizer.GetString("InStock"),
                BasketId = componentModel.BasketId,
                AddedProduct = this.orderResources.GetString("AddedProduct"),
                Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel)
            };

            var product = await this.productsRepository.GetProductAsync(componentModel.Id, componentModel.Language, null);

            if (product != null)
            {
                viewModel.ProductId = product.Id;
                viewModel.Title = product.Name;
                viewModel.BrandName = product.BrandName;
                viewModel.BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId });
                viewModel.Description = product.Description;
                viewModel.Sku = product.Sku;
                viewModel.IsProductVariant = product.PrimaryProductId.HasValue;
                viewModel.Features = product.ProductAttributes?.Select(x => new ProductFeatureViewModel { Key = x.Name, Value = string.Join(", ", x.Values.OrEmptyIfNull()) });

                var images = new List<Web.Shared.ViewModels.Images.ImageViewModel>();

                foreach (var image in product.Images.OrEmptyIfNull())
                {
                    var imageViewModel = new Web.Shared.ViewModels.Images.ImageViewModel
                    {
                        Id = image,
                        Original = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, image, ProductConstants.OriginalMaxWidth, ProductConstants.OriginalMaxHeight, true)),
                        Thumbnail = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, image, ProductConstants.ThumbnailMaxWidth, ProductConstants.ThumbnailMaxHeight, true))
                    };

                    images.Add(imageViewModel);
                }

                viewModel.Images = images;

                viewModel.Files = await this.filesModelBuilder.BuildModelAsync(new FilesComponentModel { Id = componentModel.Id, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language, Token = componentModel.Token, Files = product.Files });

                var inventory = await this.productsRepository.GetProductStockAsync(componentModel.Id);

                if (inventory != null && inventory.AvailableQuantity.HasValue && inventory.AvailableQuantity.Value > 0)
                {
                    viewModel.InStock = true;
                    viewModel.AvailableQuantity = inventory.AvailableQuantity;
                    viewModel.ExpectedDelivery = inventory.ExpectedDelivery;
                    viewModel.ExpectedDeliveryLabel = this.inventoryResources.GetString("ExpectedDeliveryLabel");
                    viewModel.RestockableInDays = inventory.RestockableInDays;
                    viewModel.RestockableInDaysLabel = this.inventoryResources.GetString("RestockableInDaysLabel");
                }

                if (viewModel.IsAuthenticated)
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
            }
            return viewModel;
        }
    }
}
