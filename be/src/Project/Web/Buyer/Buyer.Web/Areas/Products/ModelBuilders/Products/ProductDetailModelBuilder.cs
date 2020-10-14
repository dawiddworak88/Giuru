using Buyer.Web.Areas.Products.ModelBuilders.Definitions;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Buyer.Web.Shared.Images.ViewModels;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ContentGrids.Definitions;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Products
{
    public class ProductDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaService mediaService;
        private readonly LinkGenerator linkGenerator;

        public ProductDetailModelBuilder(IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ProductResources> productLocalizer,
            IOptions<AppSettings> options,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.options = options;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
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

                var images = new List<ImageViewModel>();

                foreach (var image in product.Images.OrEmptyIfNull())
                {
                    var imageViewModel = new ImageViewModel
                    { 
                        Original = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, image, ProductConstants.OriginalMaxWidth, ProductConstants.OriginalMaxHeight),
                        Thumbnail = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, image, ProductConstants.ThumbnailMaxWidth, ProductConstants.ThumbnailMaxHeight)
                    };

                    images.Add(imageViewModel);
                }

                viewModel.Images = images;

                var productVariants = await this.productsRepository.GetProductsAsync(product.ProductVariants, null, null, componentModel.Language, null, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token);

                var carouselItems = new List<ContentGridCarouselItemViewModel>();

                foreach (var productVariant in productVariants.Data.OrEmptyIfNull())
                {
                    var carouselItem = new ContentGridCarouselItemViewModel
                    {
                        Id = productVariant.Id,
                        Title = productVariant.Name,
                        ImageAlt = productVariant.Name,
                        Url = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, productVariant.Id })
                    };

                    if (productVariant.Images != null && productVariant.Images.Any())
                    {
                        carouselItem.ImageUrl = this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, productVariant.Images.FirstOrDefault(), ContentGridConstants.CarouselItemImageMaxWidth, ContentGridConstants.CarouselItemImageMaxHeight);
                    }

                    carouselItems.Add(carouselItem);
                }

                viewModel.ProductVariants = new List<ContentGridItemViewModel>
                {
                    new ContentGridItemViewModel
                    {
                        Id = product.Id,
                        Title = this.productLocalizer.GetString("ProductVariants"),
                        CarouselItems = carouselItems
                    }
                };
            }

            return viewModel;
        }
    }
}
