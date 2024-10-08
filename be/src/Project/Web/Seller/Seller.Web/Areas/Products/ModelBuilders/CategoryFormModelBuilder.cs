﻿using Foundation.Extensions.ModelBuilders;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class CategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> _categoryBaseFormModelBuilder;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMediaItemsRepository _mediaItemsRepository;
        private readonly IMediaService _mediaService;

        public CategoryFormModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryBaseFormViewModel> categoryBaseFormModelBuilder,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IMediaService mediaService)
        {
            _categoriesRepository = categoriesRepository;
            _mediaItemsRepository = mediaItemsRepository;
            _mediaService = mediaService;
            _categoryBaseFormModelBuilder = categoryBaseFormModelBuilder;
        }

        public async Task<CategoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryFormViewModel
            {
                CategoryBase = await _categoryBaseFormModelBuilder.BuildModelAsync(componentModel)
            };

            if (componentModel.Id.HasValue)
            {
                var category = await _categoriesRepository.GetCategoryAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (category is not null)
                {
                    viewModel.Id = category.Id;
                    viewModel.Name = category.Name;
                    viewModel.ParentCategoryId = category.ParentId;

                    if (category.ThumbnailMediaId.HasValue)
                    {
                        var mediaItem = await _mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, category.ThumbnailMediaId.Value);

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
                }
            }

            return viewModel;
        }
    }
}