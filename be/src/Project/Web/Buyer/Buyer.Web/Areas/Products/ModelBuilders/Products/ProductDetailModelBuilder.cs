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
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.Repositories.Media;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> _filesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IProductsRepository _productsRepository;
        private readonly IStringLocalizer<InventoryResources> _inventoryResources;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderResources;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IBasketService _basketService;
        private readonly IMediaItemsRepository _mediaItemsRepository;

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
            IMediaItemsRepository mediaItemsRepository)
        {
            _filesModelBuilder = filesModelBuilder;
            _productsRepository = productsRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _mediaService = mediaService;
            _sidebarModelBuilder = sidebarModelBuilder;
            _inventoryResources = inventoryResources;
            _linkGenerator = linkGenerator;
            _basketService = basketService;
            _orderResources = orderResources;
            _modalModelBuilder = modalModelBuilder;
            _mediaItemsRepository = mediaItemsRepository;
        }

        public async Task<ProductDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductDetailViewModel
            {
                ByLabel = _globalLocalizer.GetString("By"),
                DescriptionLabel = _globalLocalizer.GetString("Description"),
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductInformationLabel = _productLocalizer.GetString("ProductInformation"),
                PricesLabel = _globalLocalizer.GetString("Prices"),
                SuccessfullyAddedProduct = _globalLocalizer.GetString("SuccessfullyAddedProduct"),
                QuantityErrorMessage = _globalLocalizer.GetString("QuantityErrorMessage"),
                SignInToSeePricesLabel = _globalLocalizer.GetString("SignInToSeePrices"),
                SignInUrl = "#",
                UpdateBasketUrl = _linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                BasketLabel = _globalLocalizer.GetString("BasketLabel"),
                SkuLabel = _productLocalizer.GetString("Sku"),
                InStockLabel = _globalLocalizer.GetString("InStock"),
                InOutletLabel = _globalLocalizer.GetString("InOutlet"),
                BasketId = componentModel.BasketId,
                AddedProduct = _orderResources.GetString("AddedProduct"),
                Sidebar = await _sidebarModelBuilder.BuildModelAsync(componentModel),
                Modal = await _modalModelBuilder.BuildModelAsync(componentModel),
                EanLabel = _globalLocalizer.GetString("Ean"),
                OutletTitleLabel = _globalLocalizer.GetString("Discount"),
                ReadMoreText = _globalLocalizer.GetString("ReadMore"),
                ReadLessText = _globalLocalizer.GetString("ReadLess")
            };

            var product = await _productsRepository.GetProductAsync(componentModel.Id, componentModel.Language, null);

            if (product != null)
            {
                viewModel.Ean = product.Ean;
                viewModel.ProductId = product.Id;
                viewModel.Title = product.Name;
                viewModel.BrandName = product.BrandName;
                viewModel.BrandUrl = _linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId });
                viewModel.Description = product.Description;
                viewModel.Sku = product.Sku;
                viewModel.IsProductVariant = product.PrimaryProductId.HasValue;
                viewModel.Features = product.ProductAttributes?.Select(x => new ProductFeatureViewModel { Key = x.Name, Value = string.Join(", ", x.Values.OrEmptyIfNull()) });

                var imagesMediaItems = await _mediaItemsRepository.GetMediaItemsAsync(
                    componentModel.Token,
                    componentModel.Language,
                    product.Images,
                    PaginationConstants.DefaultPageIndex,
                    PaginationConstants.DefaultPageSize);

                var images = new List<ImageViewModel>();

                foreach (var imageId in product.Images.OrEmptyIfNull())
                {
                    var mediaItem = imagesMediaItems.FirstOrDefault(x =>  x.Id == imageId);

                    if (mediaItem is not null)
                    {
                        var imageViewModel = new ImageViewModel
                        {
                            ImageSrc = _mediaService.GetMediaUrl(imageId),
                            ImageAlt = mediaItem.Description,
                            MimeType = mediaItem.MimeType,
                            Sources = new List<SourceViewModel>
                            {
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(imageId, 1366) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(imageId, 470) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(imageId, 342) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(imageId, 768) }
                            }
                        };

                        images.Add(imageViewModel);
                    }
                }                

                viewModel.Images = images;

                var productFiles = await _productsRepository.GetProductFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, null, $"{nameof(ProductFile.CreatedDate)} desc");

                if (productFiles is not null)
                {
                    var fileComponentModel = new FilesComponentModel
                    {
                        Id = componentModel.Id,
                        IsAuthenticated = componentModel.IsAuthenticated,
                        Language = componentModel.Language,
                        Token = componentModel.Token,
                        SearchApiUrl = _linkGenerator.GetPathByAction("GetFiles", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                        Files = productFiles.Data.OrEmptyIfNull().Select(x => x.Id)
                    };

                    viewModel.Files = await _filesModelBuilder.BuildModelAsync(fileComponentModel);
                }

                var inventory = await _productsRepository.GetProductStockAsync(componentModel.Id);

                if (inventory is not null && inventory.AvailableQuantity.HasValue && inventory.AvailableQuantity.Value > 0)
                {
                    viewModel.InStock = true;
                    viewModel.AvailableQuantity = inventory.AvailableQuantity;
                    viewModel.ExpectedDelivery = inventory.ExpectedDelivery;
                    viewModel.ExpectedDeliveryLabel = _inventoryResources.GetString("ExpectedDeliveryLabel");
                    viewModel.RestockableInDays = inventory.RestockableInDays;
                    viewModel.RestockableInDaysLabel = _inventoryResources.GetString("RestockableInDaysLabel");
                }

                var outlet = await _productsRepository.GetProductOutletAsync(componentModel.Id);

                if (outlet is not null && outlet.AvailableQuantity.HasValue && outlet.AvailableQuantity.Value > 0)
                {
                    viewModel.InOutlet = true;
                    viewModel.OutletTitle = outlet.Title;
                    viewModel.AvailableOutletQuantity = outlet.AvailableQuantity;
                    viewModel.ExpectedOutletDelivery = outlet.ExpectedDelivery;
                }

                if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
                {
                    var basketItems = await _basketService.GetBasketAsync(componentModel.BasketId, componentModel.Token, componentModel.Language);
                    if (basketItems is not null)
                    {
                        viewModel.OrderItems = basketItems;
                    }
                }

                if (product.ProductVariants is not null)
                {
                    var productVariants = await _productsRepository.GetProductsAsync(product.ProductVariants, null, null, componentModel.Language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token, nameof(Product.CreatedDate));

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
                                Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id })
                            };

                            if (productVariant.Images != null && productVariant.Images.Any())
                            {
                                carouselItem.ImageUrl = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), CarouselGridConstants.CarouselItemImageMaxWidth);
                                carouselItem.Sources = new List<SourceViewModel>
                                {
                                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 1366) },
                                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 470) },
                                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 342) },
                                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 768) },
                                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 1366) },
                                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 470) },
                                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 342) },
                                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(productVariant.Images.FirstOrDefault(), 768) }
                                };
                            }

                            carouselItems.Add(carouselItem);
                        }

                        viewModel.ProductVariants = new List<CarouselGridItemViewModel>
                        {
                            new CarouselGridItemViewModel
                            {
                                Id = product.Id,
                                Title = _productLocalizer.GetString("ProductVariants"),
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
