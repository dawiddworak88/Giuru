using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter;
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
    public class DownloadCenterItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadCenterItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository;
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IMediaService mediaService;

        public DownloadCenterItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.downloadCenterCategoriesRepository = downloadCenterCategoriesRepository;
            this.downloadCenterRepository = downloadCenterRepository;
            this.mediaService = mediaService;
            this.mediaItemsRepository = mediaItemsRepository;
        }

        public async Task<DownloadCenterItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadCenterItemFormViewModel
            {
                Title = this.globalLocalizer.GetString("EditDownloadCenter"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NavigateToDownloadCenterLabel = this.downloadCenterLocalizer.GetString("NavigateToDownloadCenter"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                DownloadCenterUrl = this.linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                SelectCategoryLabel = this.downloadCenterLocalizer.GetString("SelectCategory"),
                OrderLabel = this.globalLocalizer.GetString("Level"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "DownloadCenterApi", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name }),
                CategoriesLabel = this.globalLocalizer.GetString("Categories"),
                FilesLabel = this.globalLocalizer.GetString("Files"),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile")
            };

            var categories = await this.downloadCenterCategoriesRepository.GetCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var downloadCenterItem = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterItem is not null)
                {
                    viewModel.Id = downloadCenterItem.Id;
                    viewModel.CategoriesIds = downloadCenterItem.CategoriesIds;

                    var file = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, downloadCenterItem.Id);

                    if (file is not null)
                    {
                        viewModel.Files = new List<FileViewModel>
                        {
                           new FileViewModel
                           {
                               Id = file.Id,
                               Url = file.MimeType.StartsWith("image") ? this.mediaService.GetMediaUrl(file.Id, Constants.PreviewMaxWidth) : this.mediaService.GetNonCdnMediaUrl(file.Id),
                               Name = file.Name,
                               MimeType = file.MimeType,
                               Filename = file.Filename,
                               Extension = file.Extension
                           }
                        };
                    }
                }
            }

            return viewModel;
        }
    }
}
