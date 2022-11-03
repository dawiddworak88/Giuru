using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Collections.Generic;
using Seller.Web.Shared.ViewModels;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateProductFormModelBuilder : IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductBaseFormViewModel> productBaseFormModelBuilder;
        private readonly IProductsRepository productsRepository;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public DuplicateProductFormModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductBaseFormViewModel> productBaseFormModelBuilder,
            IProductsRepository productsRepository,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaService)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaService;
            this.globalLocalizer = globalLocalizer;
            this.productBaseFormModelBuilder = productBaseFormModelBuilder;
        }

        public async Task<ProductFormViewModel> BuildModelAsync(DuplicateProductComponentModel componentModel)
        {
            var viewModel = new ProductFormViewModel
            {
                ProductBase = await this.productBaseFormModelBuilder.BuildModelAsync(componentModel)
            };

            if (componentModel.Id.HasValue)
            {
                var product = await this.productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (product is not null)
                {
                    viewModel.Name = $"{product.Name} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.Sku = $"{product.Sku} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.Description = product.Description;
                    viewModel.IsNew = product.IsNew;
                    viewModel.Ean = product.Ean;
                    viewModel.IsPublished = product.IsPublished;
                    viewModel.CategoryId = product.CategoryId;
                    viewModel.FormData = product.FormData;
                    viewModel.GroupIds = product.Groups;

                    var categorySchema = await this.categoriesRepository.GetCategorySchemaAsync(componentModel.Token, componentModel.Language, product.CategoryId);

                    if (categorySchema is not null)
                    {
                        viewModel.Schema = categorySchema.Schema;
                        viewModel.UiSchema = categorySchema.UiSchema;
                    }

                    if (product.PrimaryProductId.HasValue)
                    {
                        var primaryProduct = await this.productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, product.PrimaryProductId);

                        if (primaryProduct is not null)
                        {
                            viewModel.PrimaryProduct = new ListItemViewModel { Id = primaryProduct.Id, Name = primaryProduct.Name };
                        }
                    }

                    if (product.Images is not null && product.Images.Any())
                    {
                        var imageMediaItems = await this.mediaItemsRepository.GetAllMediaItemsAsync(componentModel.Token, componentModel.Language, product.Images.ToEndpointParameterString(), PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize);

                        var images = new List<FileViewModel>();

                        foreach (var mediaItem in imageMediaItems)
                        {
                            images.Add(new FileViewModel 
                            {
                                Id = mediaItem.Id,
                                Url = this.mediaService.GetMediaUrl(mediaItem.Id, Constants.PreviewMaxWidth),
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
                        var fileMediaItems = await this.mediaItemsRepository.GetAllMediaItemsAsync(componentModel.Token, componentModel.Language, product.Files.ToEndpointParameterString(), PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize);

                        var files = new List<FileViewModel>();

                        foreach (var file in fileMediaItems)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = this.mediaService.GetMediaUrl(file.Id, Constants.PreviewMaxWidth),
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
