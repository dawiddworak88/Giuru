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
using Foundation.Media.Services.MediaServices;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
using System;
using Buyer.Web.Shared.DomainModels.Prices;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Foundation.GenericRepository.Definitions;
using Buyer.Web.Shared.ViewModels.Toasts;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<PriceComponentModel, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> _filesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IModelBuilder<SuccessAddProductToBasketViewModel> _toastSuccessAddProductToBasket;
        private readonly IProductsRepository _productsRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IStringLocalizer<InventoryResources> _inventoryResources;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderResources;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IBasketService _basketService;
        private readonly IMediaItemsRepository _mediaItemsRepository;
        private readonly IPriceService _priceService;
        private readonly IOptions<AppSettings> _options;
        private readonly IProductsService _productsService;
        private readonly IProductColorsService _productColorsService;

        public ProductDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IModelBuilder<SuccessAddProductToBasketViewModel> toastSuccessAddProductToBasket,
            IProductsRepository productsRepository,
            IOutletRepository outletRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IStringLocalizer<InventoryResources> inventoryResources,
            IStringLocalizer<OrderResources> orderResources,
            IMediaService mediaService,
            IBasketService basketService,
            LinkGenerator linkGenerator,
            IMediaItemsRepository mediaItemsRepository,
            IPriceService priceService,
            IOptions<AppSettings> options,
            IProductsService productsService,
            IProductColorsService productColorsService)
        {
            _filesModelBuilder = filesModelBuilder;
            _productsRepository = productsRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _outletRepository = outletRepository;
            _mediaService = mediaService;
            _sidebarModelBuilder = sidebarModelBuilder;
            _inventoryResources = inventoryResources;
            _linkGenerator = linkGenerator;
            _basketService = basketService;
            _orderResources = orderResources;
            _modalModelBuilder = modalModelBuilder;
            _toastSuccessAddProductToBasket = toastSuccessAddProductToBasket;
            _mediaItemsRepository = mediaItemsRepository;
            _priceService = priceService;
            _options = options;
            _productsService = productsService;
            _productColorsService = productColorsService;
        }

        public async Task<ProductDetailViewModel> BuildModelAsync(PriceComponentModel componentModel)
        {
            var viewModel = new ProductDetailViewModel
            {
                ByLabel = _globalLocalizer.GetString("By"),
                DescriptionLabel = _globalLocalizer.GetString("Description"),
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductInformationLabel = _productLocalizer.GetString("ProductInformation"),
                PricesLabel = _globalLocalizer.GetString("Prices"),
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
                ReadLessText = _globalLocalizer.GetString("ReadLess"),
                SeeMoreText = _globalLocalizer.GetString("SeeMoreText"),
                SeeLessText = _globalLocalizer.GetString("SeeLessText"),
                MaxAllowedOrderQuantity = _options.Value.MaxAllowedOrderQuantity,
                MaxAllowedOrderQuantityErrorMessage = _globalLocalizer.GetString("MaxAllowedOrderQuantity"),
                CopiedText = _globalLocalizer.GetString("CopiedText"),
                CopyTextError = _globalLocalizer.GetString("CopyTextError"),
                CopyToClipboardText = _globalLocalizer.GetString("CopyToClipboardText"),
                GetProductPriceUrl = _linkGenerator.GetPathByAction("GetPrice", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                MinOrderQuantityErrorMessage = _globalLocalizer.GetString("MinOrderQuantity")
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
                viewModel.ToastSuccessAddProductToBasket = _toastSuccessAddProductToBasket.BuildModel();

                var outlet = await _productsRepository.GetProductOutletAsync(componentModel.Id);

                if (outlet is not null && outlet.AvailableQuantity.HasValue && outlet.AvailableQuantity.Value > 0)
                {
                    viewModel.InOutlet = true;
                    viewModel.OutletTitle = outlet.Title;
                    viewModel.AvailableOutletQuantity = outlet.AvailableQuantity;
                    viewModel.ExpectedOutletDelivery = outlet.ExpectedDelivery;
                }

                if (product.PrimaryProductId.HasValue &&
                    string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                {
                    var price = await _priceService.GetPrice(
                        _options.Value.GrulaAccessToken,
                        DateTime.UtcNow,
                        new PriceProduct
                        {
                            PrimarySku = product.PrimaryProductSku,
                            FabricsGroup = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                            ExtraPacking = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys).ToYesOrNo(),
                            SleepAreaSize = _productsService.GetSleepAreaSize(product.ProductAttributes),
                            PaletteSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                            Size = _productsService.GetSize(product.ProductAttributes),
                            PointsOfLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                            LampshadeType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                            LampshadeSize = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                            LinearLight = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                            Mirror = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                            Shape = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                            PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                            SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                            ShelfType = _productsService.GetFirstAvailableAttributeValue(product.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys)
                        },
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

                    if (price is not null)
                    {
                        viewModel.Price = new ProductPriceViewModel
                        {
                            Current = price.CurrentPrice,
                            Currency = price.CurrencyCode,
                        };
                    }
                }

                var imagesMediaItems = await _mediaItemsRepository.GetMediaItemsAsync(
                    componentModel.Token,
                    componentModel.Language,
                    product.Images,
                    PaginationConstants.DefaultPageIndex,
                    PaginationConstants.DefaultPageSize);

                var mediaItems = new List<ProductMediaItemViewModel>();

                foreach (var mediaItemId in product.Images.OrEmptyIfNull())
                {
                    var mediaItem = imagesMediaItems.FirstOrDefault(x =>  x.Id == mediaItemId);

                    if (mediaItem is not null)
                    {
                        var mediaItemViewModel = new ProductMediaItemViewModel
                        {
                            MediaSrc = _mediaService.GetMediaUrl(mediaItemId),
                            MediaAlt = mediaItem.Description,
                            MimeType = mediaItem.MimeType,
                            Sources = new List<SourceViewModel>
                            {
                                new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = _mediaService.GetMediaUrl(mediaItemId, 1366) },
                                new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = _mediaService.GetMediaUrl(mediaItemId, 470) },
                                new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = _mediaService.GetMediaUrl(mediaItemId, 342) },
                                new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = _mediaService.GetMediaUrl(mediaItemId, 768) }
                            }
                        };

                        mediaItems.Add(mediaItemViewModel);
                    }
                }                

                viewModel.MediaItems = mediaItems;

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
                    var productVariants = await _productsRepository.GetProductsAsync(product.ProductVariants, null, null, componentModel.Language, null, false, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token, SortingConstants.Default);

                    if (productVariants != null)
                    {
                        var outletProductVariants = await _outletRepository.GetOutletProductsByProductsIdAsync(componentModel.Token, componentModel.Language, productVariants.Data.Select(x => x.Id));

                        var carouselItems = new List<CarouselGridCarouselItemViewModel>();

                        var prices = Enumerable.Empty<Price>();

                        if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                        {
                            var priceProducts = productVariants.Data.Select(async x => new PriceProduct
                            {
                                PrimarySku = x.PrimaryProductSku,
                                FabricsGroup = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePriceGroupAttributeKeys),
                                ExtraPacking = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleExtraPackingAttributeKeys),
                                SleepAreaSize = _productsService.GetSleepAreaSize(x.ProductAttributes),
                                PaletteSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePaletteSizeAttributeKeys),
                                Size = _productsService.GetSize(x.ProductAttributes),
                                PointsOfLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePointsOfLightAttributeKeys),
                                LampshadeType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeTypeAttributeKeys),
                                LampshadeSize = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLampshadeSizeAttributeKeys),
                                LinearLight = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleLinearLightAttributeKeys).ToYesOrNo(),
                                Mirror = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleMirrorAttributeKeys).ToYesOrNo(),
                                Shape = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShapeAttributeKeys),
                                PrimaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossiblePrimaryColorAttributeKeys)),
                                SecondaryColor = await _productColorsService.ToEnglishAsync(_productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleSecondaryColorAttributeKeys)),
                                ShelfType = _productsService.GetFirstAvailableAttributeValue(x.ProductAttributes, _options.Value.PossibleShelfTypeAttributeKeys)
                            });

                            prices = await _priceService.GetPrices(
                               _options.Value.GrulaAccessToken,
                               DateTime.UtcNow,
                               await Task.WhenAll(priceProducts),
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

                        for (var i = 0; i < productVariants.Data.Count(); i++) 
                        {
                            var productVariant = productVariants.Data.ElementAtOrDefault(i);

                            if (productVariant is null)
                            {
                                continue;
                            }  

                            var carouselItem = new CarouselGridCarouselItemViewModel
                            {
                                Id = productVariant.Id,
                                Title = productVariant.Name,
                                Subtitle = productVariant.Sku,
                                ImageAlt = productVariant.Name,
                                Url = _linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id })
                            };

                            var productVariantPrice = prices.ElementAtOrDefault(i);

                            if (productVariantPrice is not null)
                            {
                                carouselItem.Price = new CarouselGridPriceViewModel
                                {
                                    Current = productVariantPrice.CurrentPrice,
                                    Currency = productVariantPrice.CurrencyCode
                                };
                            }

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
