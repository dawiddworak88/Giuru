﻿using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.FileTypeServices;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenterCategories;
using Seller.Web.Areas.DownloadCenter.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ModelBuilders
{
    public class DownloadCenterCategoryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterCategoryFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IMediaService mediaService;
        private readonly IFileTypeService fileTypeService;

        public DownloadCenterCategoryFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IMediaService mediaService,
            IFileTypeService fileTypeService,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.downloadCenterCategoriesRepository = downloadCenterCategoriesRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.mediaService = mediaService;
            this.fileTypeService = fileTypeService;
        }

        public async Task<DownloadCenterCategoryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterCategoryFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.downloadCenterLocalizer.GetString("EditCategory"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                ParentCategoryLabel = this.downloadCenterLocalizer.GetString("ParentCategory"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "DownloadCenterCategoriesApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToCategoriesLabel = this.downloadCenterLocalizer.GetString("NavigateToCategoriesLabel"),
                CategoriesUrl = this.linkGenerator.GetPathByAction("Index", "DownloadCenterCategories", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                VisibleLabel = this.downloadCenterLocalizer.GetString("Visible")
            };

            var categories = await this.downloadCenterCategoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.ParentCategories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var category = categories.Where(x => x.Id == componentModel.Id).FirstOrDefault();

                if (category is not null)
                {
                    viewModel.Id = category.Id;
                    viewModel.Name = category.Name;
                    viewModel.ParentCategoryId = category.ParentCategoryId;
                    viewModel.IsVisible = category.IsVisible;

                    if (category.Files.OrEmptyIfNull().Any())
                    {
                        var categoryFiles = new List<FileViewModel>();

                        var files = await this.mediaItemsRepository.GetAllMediaItemsAsync(componentModel.Token, componentModel.Language, category.Files.Distinct().ToEndpointParameterString(), PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize);

                        foreach (var file in files)
                        {
                            var categoryFile = new FileViewModel
                            {
                                Id = file.Id,
                                Url = fileTypeService.IsImage(file.MimeType) ? this.mediaService.GetMediaUrl(file.Id, Constants.PreviewMaxWidth) : this.mediaService.GetNonCdnMediaUrl(file.Id),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename,
                                Extension = file.Extension
                            };

                            categoryFiles.Add(categoryFile);
                        }

                        viewModel.Files = categoryFiles;
                    }
                }
            }

            return viewModel;
        }
    }
}
