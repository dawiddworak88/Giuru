using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Files;
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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;
        private readonly ICdnService cdnService;

        public ProductDetailModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IMediaItemsRepository filesRepository,
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator,
            ICdnService cdnService)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.mediaRepository = filesRepository;
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
            this.cdnService = cdnService;
        }

        public async Task<ProductDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductDetailViewModel
            {
                ByLabel = this.globalLocalizer.GetString("By"),
                DescriptionLabel = this.globalLocalizer.GetString("Description"),
                InStockLabel = this.globalLocalizer.GetString("InStock"),
                IsAuthenticated = componentModel.IsAuthenticated,
                ProductInformationLabel = this.productLocalizer.GetString("ProductInformation"),
                PricesLabel = this.globalLocalizer.GetString("Prices"),
                SignInToSeePricesLabel = this.globalLocalizer.GetString("SignInToSeePrices"),
                SignInUrl = "#",
                SkuLabel = this.productLocalizer.GetString("Sku"),
            };

            var product = await this.productsRepository.GetProductAsync(componentModel.Id, componentModel.Language, componentModel.Token);

            if (product != null)
            {
                viewModel.Title = product.Name;
                viewModel.BrandName = product.BrandName;
                viewModel.BrandUrl = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = product.SellerId });
                viewModel.Description = product.Description;
                viewModel.Sku = product.Sku;
                viewModel.Features = product.ProductAttributes?.Select(x => new ProductFeatureViewModel { Key = x.Name, Value = string.Join(", ", x.Values.OrEmptyIfNull()) });

                var images = new List<ImageViewModel>();

                foreach (var image in product.Images.OrEmptyIfNull())
                {
                    var imageViewModel = new ImageViewModel
                    { 
                        Original = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, image, ProductConstants.OriginalMaxWidth, ProductConstants.OriginalMaxHeight, true)),
                        Thumbnail = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, image, ProductConstants.ThumbnailMaxWidth, ProductConstants.ThumbnailMaxHeight, true))
                    };

                    images.Add(imageViewModel);
                }

                viewModel.Images = images;

                viewModel.Files = await this.filesModelBuilder.BuildModelAsync(new FilesComponentModel { Id = componentModel.Id, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language, Token = componentModel.Token, Files = product.Files });

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
