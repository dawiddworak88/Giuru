using Foundation.Extensions.ExtensionMethods;
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
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class DuplicateCategoryFormModelBuilder : IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> _categoryBaseFormModelBuilder;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMediaItemsRepository _mediaItemsRepository;
        private readonly IStringLocalizer _globalLocalizer;
        private readonly IMediaService _mediaService;

        public DuplicateCategoryFormModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> categoryBaseFormModelBuilder,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaService)
        {
            _categoriesRepository = categoriesRepository;
            _mediaItemsRepository = mediaItemsRepository;
            _globalLocalizer = globalLocalizer;
            _mediaService = mediaService;
            _categoryBaseFormModelBuilder = categoryBaseFormModelBuilder;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(DuplicateCategoryComponentModel componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
                CategoryBase = await _categoryBaseFormModelBuilder.BuildModelAsync(componentModel),
                Language = componentModel.Language,
            };

            if (componentModel.Id.HasValue)
            {
                var category = await _categoriesRepository.GetCategoryAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (category is not null)
                {
                    viewModel.Name = $"{category.Name} {_globalLocalizer.GetString("Copy")}";
                    viewModel.ParentCategoryId = category.ParentId;

                    if (category.ThumbnailMediaId.HasValue)
                    {
                        var mediaItem = await _mediaItemsRepository.GetMediaItemAsync(componentModel.Token,componentModel.Language,category.ThumbnailMediaId.Value);

                        if (mediaItem is not null)
                        {
                            viewModel.Files = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = mediaItem.Id,
                                    Url = _mediaService.GetMediaUrl(mediaItem.Id, Constants.PreviewMaxWidth),
                                    Name = mediaItem.Name,
                                    MimeType = mediaItem.MimeType,
                                    Filename = mediaItem.Filename,
                                    Extension = mediaItem.Extension
                                }
                            };
                        }
                    }

                    var categorySchemas = await _categoriesRepository.GetCategorySchemasAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                    if (categorySchemas is not null)
                    {
                        viewModel.Schemas = categorySchemas.Schemas.OrEmptyIfNull().Select(x => new CategorySchemaViewModel
                        {
                            Id = x.Id,
                            Schema = x.Schema,
                            UiSchema = x.UiSchema,
                            Language = x.Language
                        });
                    }
                }
            }

            return viewModel;
        }
    }
}