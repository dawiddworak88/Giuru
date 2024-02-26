using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Collections.Generic;
using Seller.Web.Shared.ViewModels;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using System;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.Definitions;
using Seller.Web.Areas.Products.Definitions;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductFormViewModel>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMediaItemsRepository _mediaItemsRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly IMediaService _mediaService;
        private readonly LinkGenerator _linkGenerator;

        public ProductFormModelBuilder(
            IProductsRepository productsRepository,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            _productsRepository = productsRepository;
            _categoriesRepository = categoriesRepository;
            _mediaItemsRepository = mediaItemsRepository;
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _mediaService = mediaService;
            _linkGenerator = linkGenerator;
        }

        public async Task<ProductFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _productLocalizer.GetString("EditProduct"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                NameLabel = _globalLocalizer.GetString("NameLabel"),
                DescriptionLabel = _globalLocalizer.GetString("DescriptionLabel"),
                NameRequiredErrorMessage = _globalLocalizer.GetString("NameRequiredErrorMessage"),
                EnterNameText = _globalLocalizer.GetString("EnterNameText"),
                EnterSkuText = _productLocalizer.GetString("EnterSkuText"),
                SkuRequiredErrorMessage = _productLocalizer.GetString("SkuRequiredErrorMessage"),
                SkuLabel = _productLocalizer.GetString("SkuLabel"),
                DaysToFulfilmentLabel = _productLocalizer.GetString("DaysToFulfilmentLabel"),
                DropFilesLabel = _globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = _globalLocalizer.GetString("DropOrSelectFile"),
                DeleteLabel = _globalLocalizer.GetString("Delete"),
                SaveMediaUrl = _linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkUrl = _linkGenerator.GetPathByAction("PostChunk", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaChunkCompleteUrl = _linkGenerator.GetPathByAction("PostChunksComplete", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                IsUploadInChunksEnabled = true,
                ChunkSize = MediaConstants.DefaultChunkSize,
                VideoFileSizeLimit = ProductsConstants.Limits.VideoFileSizeLimit,
                FileSizeLimitErrorMessage = _globalLocalizer.GetString("FileSizeLimit"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductPicturesLabel = _productLocalizer.GetString("ProductPicturesLabel"),
                ProductFilesLabel = _productLocalizer.GetString("ProductFilesLabel"),
                SelectCategoryLabel = _productLocalizer.GetString("SelectCategory"),
                SelectPrimaryProductLabel = _productLocalizer.GetString("SelectPrimaryProduct"),
                IsNewLabel = _productLocalizer.GetString("IsNew"),
                IsPublishedLabel = _productLocalizer.GetString("IsPublished"),
                GetCategorySchemaUrl = _linkGenerator.GetPathByAction("Get", "CategorySchemasApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductsUrl = _linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                EanLabel = _globalLocalizer.GetString("Ean"),
                NavigateToProductsLabel = _productLocalizer.GetString("NavigateToProductsLabel"),
                ProductsSuggestionUrl = _linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            var categories = await _categoriesRepository.GetAllCategoriesAsync(
                componentModel.Token, componentModel.Language, true, $"{nameof(Category.Level)}");

            if (categories != null)
            {
                viewModel.Categories = categories.OrderBy(x => x.Level).ThenBy(x => x.Name).Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var primaryProducts = await _productsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, null, false, componentModel.SellerId, Constants.ProductsSuggestionDefaultPageIndex, Constants.ProductsSuggestionDefaultItemsPerPage, $"{nameof(Product.Name)} ASC");

            if (primaryProducts != null)
            {
                viewModel.PrimaryProducts = primaryProducts.Data.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var product = await _productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (product != null)
                {
                    viewModel.Id = product.Id;
                    viewModel.Name = product.Name;
                    viewModel.Sku = product.Sku;
                    viewModel.Description = product.Description;
                    viewModel.IsPublished = product.IsPublished;
                    viewModel.IsNew = product.IsNew;
                    viewModel.CategoryId = product.CategoryId;
                    viewModel.FormData = product.FormData;
                    viewModel.Ean = product.Ean;
                    viewModel.DaysToFulfilment = product.DaysToFunfilment;

                    var categorySchema = await _categoriesRepository.GetCategorySchemasAsync(
                        componentModel.Token,
                        componentModel.Language,
                        product.CategoryId);

                    if (categorySchema is not null && categorySchema.Schemas.OrEmptyIfNull().Any())
                    {
                        viewModel.Schema = categorySchema.Schemas.FirstOrDefault(x => x.Language == componentModel.Language)?.Schema ?? categorySchema.Schemas.FirstOrDefault()?.Schema;
                        viewModel.UiSchema = categorySchema.Schemas.FirstOrDefault(x => x.Language == componentModel.Language)?.UiSchema ?? categorySchema.Schemas.FirstOrDefault()?.UiSchema;
                    }

                    if (product.PrimaryProductId.HasValue)
                    {
                        var primaryProduct = await _productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, product.PrimaryProductId);

                        if (primaryProduct is not null)
                        {
                            viewModel.PrimaryProduct = new ListItemViewModel { Id = primaryProduct.Id, Name = primaryProduct.Name };
                        }
                    }

                    if (product.Images != null && product.Images.Any())
                    {
                        var imageMediaItems = await _mediaItemsRepository.GetAllMediaItemsAsync(
                            componentModel.Token,
                            componentModel.Language,
                            product.Images.Distinct().ToEndpointParameterString(),
                            PaginationConstants.DefaultPageIndex,
                            PaginationConstants.DefaultPageSize);

                        var images = new List<FileViewModel>();

                        foreach (var mediaItem in imageMediaItems)
                        {
                            images.Add(new FileViewModel 
                            {
                                Id = mediaItem.Id,
                                Url = _mediaService.GetMediaUrl(mediaItem.Id, Constants.PreviewMaxWidth),
                                Name = mediaItem.Name,
                                MimeType = mediaItem.MimeType,
                                Filename = mediaItem.Filename,
                                Extension = mediaItem.Extension
                            });
                        }

                        viewModel.Images = images;
                    }

                    if (product.Files != null && product.Files.Any())
                    {
                        var fileMediaItems = await _mediaItemsRepository.GetAllMediaItemsAsync(
                            componentModel.Token,
                            componentModel.Language,
                            product.Files.Distinct().ToEndpointParameterString(),
                            PaginationConstants.DefaultPageIndex,
                            PaginationConstants.DefaultPageSize);

                        var files = new List<FileViewModel>();

                        foreach (var file in fileMediaItems)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = _mediaService.GetMediaUrl(file.Id, Constants.PreviewMaxWidth),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename,
                                Extension = file.Extension
                            });
                        }

                        viewModel.Files = files;
                    }
                }
            }

            return viewModel;
        }
    }
}
