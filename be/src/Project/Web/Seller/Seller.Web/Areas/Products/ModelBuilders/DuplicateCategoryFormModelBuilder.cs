using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateCategoryFormModelBuilder : IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> categoryBaseFormModelBuilder;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IMediaService mediaService;

        public DuplicateCategoryFormModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> categoryBaseFormModelBuilder,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaService)
        {
            this.categoriesRepository = categoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaService;
            this.categoryBaseFormModelBuilder = categoryBaseFormModelBuilder;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(DuplicateCategoryComponentModel componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
                CategoryBase = await this.categoryBaseFormModelBuilder.BuildModelAsync(componentModel),
                Language = componentModel.Language,
            };

            if (componentModel.Id.HasValue)
            {
                var category = await this.categoriesRepository.GetCategoryAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (category is not null)
                {
                    viewModel.Name = $"{category.Name} {this.globalLocalizer.GetString("Copy")}";
                    viewModel.ParentCategoryId = category.ParentId;

                    if (category.ThumbnailMediaId.HasValue)
                    {
                        var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token,componentModel.Language,category.ThumbnailMediaId.Value);

                        if (mediaItem is not null)
                        {
                            viewModel.Files = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = mediaItem.Id,
                                    Url = this.mediaService.GetMediaUrl(mediaItem.Id, Constants.PreviewMaxWidth),
                                    Name = mediaItem.Name,
                                    MimeType = mediaItem.MimeType,
                                    Filename = mediaItem.Filename,
                                    Extension = mediaItem.Extension
                                }
                            };
                        }
                    }

                    var categorySchemas = await this.categoriesRepository.GetCategorySchemasAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                    if (categorySchemas is not null)
                    {
                        viewModel.Schemas = categorySchemas.Schemas;                        
                    }
                }
            }

            return viewModel;
        }
    }
}