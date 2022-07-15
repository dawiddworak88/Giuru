using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Areas.DownloadCenter.ViewModel;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ModelBuilders
{
    public class CategoryDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoryDetailsViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly IMediaService mediaService;
        private readonly IMediaItemsRepository mediaItemsRepository;

        public CategoryDetailsModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IDownloadCenterRepository downloadCenterRepository,
            IMediaItemsRepository mediaItemsRepository,
            IMediaService mediaService)
        {
            this.globalLocalizer = globalLocalizer;
            this.downloadCenterRepository = downloadCenterRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.mediaService = mediaService;
        }

        public async Task<CategoryDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoryDetailsViewModel
            {
                DownloadSelectedLabel = this.globalLocalizer.GetString("DownloadSelected"),
                DownloadEverythingLabel = this.globalLocalizer.GetString("DownloadEverything"),
                NoCategoriesLabel = this.globalLocalizer.GetString("NoCategories")
            };

            if (componentModel.Id.HasValue)
            {
                var downloadCenterCategory = await this.downloadCenterRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (downloadCenterCategory is not null)
                {
                    viewModel.Title = downloadCenterCategory.CategoryName;
                    viewModel.Categories = downloadCenterCategory.Categories.OrEmptyIfNull().Select(x => new CategoryDetailViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = x.Url
                    });

                    if (downloadCenterCategory.Files.OrEmptyIfNull().Any())
                    {
                        var files = new List<FileViewModel>();

                        var downloadCenterCategoryFiles = await this.mediaItemsRepository.GetMediaItemsAsync(componentModel.Token, componentModel.Language, downloadCenterCategory.Files.Distinct(), PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize);

                        foreach (var file in downloadCenterCategoryFiles.OrEmptyIfNull())
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = this.mediaService.GetMediaUrl(file.Id, 200),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename
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
