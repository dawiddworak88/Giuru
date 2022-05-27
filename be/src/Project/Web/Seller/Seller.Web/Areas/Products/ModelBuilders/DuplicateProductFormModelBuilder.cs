using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.Extensions.Services.MediaServices;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using Seller.Web.Shared.ViewModels;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateProductFormModelBuilder : IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public DuplicateProductFormModelBuilder(
            IProductsRepository productsRepository,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductFormViewModel> BuildModelAsync(DuplicateProductComponentModel componentModel)
        {
            var viewModel = new ProductFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.productLocalizer.GetString("EditProduct"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NameLabel = this.globalLocalizer.GetString("NameLabel"),
                DescriptionLabel = this.globalLocalizer.GetString("DescriptionLabel"),
                NameRequiredErrorMessage = this.globalLocalizer.GetString("NameRequiredErrorMessage"),
                EnterNameText = this.globalLocalizer.GetString("EnterNameText"),
                EnterSkuText = this.productLocalizer.GetString("EnterSkuText"),
                SkuRequiredErrorMessage = this.productLocalizer.GetString("SkuRequiredErrorMessage"),
                SkuLabel = this.productLocalizer.GetString("SkuLabel"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductPicturesLabel = this.productLocalizer.GetString("ProductPicturesLabel"),
                ProductFilesLabel = this.productLocalizer.GetString("ProductFilesLabel"),
                SelectCategoryLabel = this.productLocalizer.GetString("SelectCategory"),
                SelectPrimaryProductLabel = this.productLocalizer.GetString("SelectPrimaryProduct"),
                IsNewLabel = this.productLocalizer.GetString("IsNew"),
                IsPublishedLabel = this.productLocalizer.GetString("IsPublished"),
                GetCategorySchemaUrl = this.linkGenerator.GetPathByAction("Get", "CategorySchemasApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductsUrl = this.linkGenerator.GetPathByAction("Index", "Products", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                EanLabel = this.globalLocalizer.GetString("Ean"),
                NavigateToProductsLabel = this.productLocalizer.GetString("NavigateToProductsLabel"),
                ProductsSuggestionUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(
                componentModel.Token,
                componentModel.Language,
                true,
                $"{nameof(Category.Level)}");

            if (categories != null)
            {
                viewModel.Categories = categories.OrderBy(x => x.Level).ThenBy(x => x.Name).Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var primaryProducts = await this.productsRepository.GetProductsAsync(componentModel.Token, componentModel.Language, null, false, componentModel.SellerId, Constants.ProductsSuggestionDefaultPageIndex, Constants.ProductsSuggestionDefaultItemsPerPage, $"{nameof(Product.Name)} ASC");

            if (primaryProducts != null)
            {
                viewModel.PrimaryProducts = primaryProducts.Data.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var product = await this.productsRepository.GetProductAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                if (product != null)
                {
                    viewModel.Name = $"{product.Name} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.Sku = $"{product.Sku} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.Description = product.Description;
                    viewModel.IsNew = product.IsNew;
                    viewModel.Ean = product.Ean;
                    viewModel.IsPublished = product.IsPublished;
                    viewModel.CategoryId = product.CategoryId;
                    viewModel.FormData = product.FormData;

                    var categorySchema = await this.categoriesRepository.GetCategorySchemaAsync(
                        componentModel.Token,
                        componentModel.Language,
                        product.CategoryId);

                    if (categorySchema != null)
                    {
                        viewModel.Schema = categorySchema.Schema;
                        viewModel.UiSchema = categorySchema.UiSchema;
                    }

                    var primaryProduct = await this.productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, product.PrimaryProductId);

                    if (primaryProduct is not null)
                    {
                        viewModel.PrimaryProduct = new ListItemViewModel { Id = primaryProduct.Id, Name = primaryProduct.Name };
                    }

                    if (product.Images != null && product.Images.Any())
                    {
                        var imageMediaItems = await this.mediaItemsRepository.GetAllMediaItemsAsync(
                            componentModel.Token,
                            componentModel.Language,
                            product.Images.ToEndpointParameterString(),
                            PaginationConstants.DefaultPageIndex,
                            PaginationConstants.DefaultPageSize);

                        var images = new List<FileViewModel>();

                        foreach (var mediaItem in imageMediaItems)
                        {
                            images.Add(new FileViewModel 
                            {
                                Id = mediaItem.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, mediaItem.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
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
                        var fileMediaItems = await this.mediaItemsRepository.GetAllMediaItemsAsync(
                            componentModel.Token,
                            componentModel.Language,
                            product.Files.ToEndpointParameterString(),
                            PaginationConstants.DefaultPageIndex,
                            PaginationConstants.DefaultPageSize);

                        var files = new List<FileViewModel>();

                        foreach (var file in fileMediaItems)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, file.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
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
