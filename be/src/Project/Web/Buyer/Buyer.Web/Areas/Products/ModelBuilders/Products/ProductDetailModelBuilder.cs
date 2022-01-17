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
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.CarouselGrids.Definitions;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
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

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<InventoryResources> inventoryResources;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly ICdnService cdnService;
        private readonly IBasketRepository basketRepository;

        public ProductDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IStringLocalizer<InventoryResources> inventoryResources,
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
            this.inventoryResources = inventoryResources;
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
            this.cdnService = cdnService;
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
                InStockLabel = this.globalLocalizer.GetString("InStock")
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

                var images = new List<ImageViewModel>();

                foreach (var image in product.Images.OrEmptyIfNull())
                {
                    var imageViewModel = new ImageViewModel
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

                if (product.ProductVariants != null)
                {
                    var productVariants = await this.productsRepository.GetProductsAsync(product.ProductVariants, null, null, componentModel.Language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token, nameof(Product.CreatedDate));

                    if (productVariants != null)
                    {
                        var carouselItems = new List<CarouselGridCarouselItemViewModel>();

                        foreach (var productVariant in productVariants.Data.OrEmptyIfNull())
                        {
                            var carouselItem = new CarouselGridCarouselItemViewModel
                            {
                                Id = productVariant.Id,
                                Title = productVariant.Name,
                                Subtitle = productVariant.Sku,
                                ImageAlt = productVariant.Name,
                                Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id })
                            };

                            if (productVariant.Images != null && productVariant.Images.Any())
                            {
                                carouselItem.ImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, productVariant.Images.FirstOrDefault(), CarouselGridConstants.CarouselItemImageMaxWidth, CarouselGridConstants.CarouselItemImageMaxHeight, true));
                            }

                            carouselItems.Add(carouselItem);
                        }

                        viewModel.ProductVariants = new List<CarouselGridItemViewModel>
                        {
                            new CarouselGridItemViewModel
                            {
                                Id = product.Id,
                                Title = this.productLocalizer.GetString("ProductVariants"),
                                CarouselItems = carouselItems
                            }
                        };
                    }
                }
            }

            return viewModel;
        }
    }
}
