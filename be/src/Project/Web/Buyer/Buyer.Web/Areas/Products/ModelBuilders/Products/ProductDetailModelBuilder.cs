using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.GenericRepository.Paginations;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.PageContent.Components.CarouselGrids.Definitions;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using ImageViewModel = Buyer.Web.Shared.ViewModels.Images.ImageViewModel;
using Foundation.Media.Services.MediaServices;
using Microsoft.Extensions.Logging;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<InventoryResources> inventoryResources;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderResources;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketService basketService;
        private readonly ILogger<ProductDetailModelBuilder> logger;

        public ProductDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IStringLocalizer<InventoryResources> inventoryResources,
            IStringLocalizer<OrderResources> orderResources,
            IMediaService mediaService,
            IBasketService basketService,
            LinkGenerator linkGenerator,
            ILogger<ProductDetailModelBuilder> logger)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaService = mediaService;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.inventoryResources = inventoryResources;
            this.linkGenerator = linkGenerator;
            this.basketService = basketService;
            this.orderResources = orderResources;
            this.modalModelBuilder = modalModelBuilder;
            this.logger = logger;
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
                QuantityErrorMessage = this.globalLocalizer.GetString("QuantityErrorMessage"),
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                SignInUrl = "#",
                UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                BasketLabel = this.globalLocalizer.GetString("BasketLabel"),
                SkuLabel = this.productLocalizer.GetString("Sku"),
                InStockLabel = this.globalLocalizer.GetString("InStock"),
                InOutletLabel = this.globalLocalizer.GetString("InOutlet"),
                BasketId = componentModel.BasketId,
                AddedProduct = this.orderResources.GetString("AddedProduct"),
                Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel),
                Modal = await this.modalModelBuilder.BuildModelAsync(componentModel),
                EanLabel = this.globalLocalizer.GetString("Ean"),
                OutletTitleLabel = this.globalLocalizer.GetString("Discount"),
                ReadMoreText = this.globalLocalizer.GetString("ReadMore"),
                ReadLessText = this.globalLocalizer.GetString("ReadLess")
            };

            var product = await this.productsRepository.GetProductAsync(componentModel.Id, componentModel.Language, null);

            if (product != null)
            {
                viewModel.Ean = product.Ean;
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
                        Original = this.mediaService.GetMediaUrl(image, ProductConstants.OriginalMaxWidth),
                        Thumbnail = this.mediaService.GetMediaUrl(image, ProductConstants.ThumbnailMaxWidth)
                    };

                    images.Add(imageViewModel);
                }

                viewModel.Images = images;

                this.logger.LogError("ProductDetailModelBuilder 132: " + product.Files.OrEmptyIfNull().Count());

                viewModel.Files = await this.filesModelBuilder.BuildModelAsync(new FilesComponentModel { Id = componentModel.Id, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language, Token = componentModel.Token, Files = product.Files });

                this.logger.LogError("ProductDetailModelBuilder 136: " + viewModel.Files.Files.OrEmptyIfNull().Count());

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

                var outlet = await this.productsRepository.GetProductOutletAsync(componentModel.Id);

                if (outlet is not null && outlet.AvailableQuantity.HasValue && outlet.AvailableQuantity.Value > 0)
                {
                    viewModel.InOutlet = true;
                    viewModel.OutletTitle = outlet.Title;
                    viewModel.AvailableOutletQuantity = outlet.AvailableQuantity;
                    viewModel.ExpectedOutletDelivery = outlet.ExpectedDelivery;
                }

                if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
                {
                    var basketItems = await this.basketService.GetBasketAsync(componentModel.BasketId, componentModel.Token, componentModel.Language);
                    if (basketItems is not null)
                    {
                        viewModel.OrderItems = basketItems;
                    }
                }

                if (product.ProductVariants is not null)
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
                                carouselItem.ImageUrl = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), CarouselGridConstants.CarouselItemImageMaxWidth);
                                carouselItem.Sources = new List<SourceViewModel>
                                {
                                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 1366) },
                                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 470) },
                                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 342) },
                                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 768) },
                                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 1366) },
                                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 470) },
                                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 342) },
                                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 768) }
                                };
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
