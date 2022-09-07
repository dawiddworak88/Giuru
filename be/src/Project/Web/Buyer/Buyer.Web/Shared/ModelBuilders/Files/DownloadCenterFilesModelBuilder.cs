using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.Media.Services.MediaServices;
using System.Linq;
using Buyer.Web.Shared.Definitions.Files;

namespace Buyer.Web.Shared.ModelBuilders.Files
{
    public class DownloadCenterFilesModelBuilder : IAsyncComponentModelBuilder<DownloadCenterFilesComponentModel, DownloadCenterFilesViewModel>
    {
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public DownloadCenterFilesModelBuilder(
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaHelperService)
        {
            this.mediaRepository = mediaRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaHelperService;
        }

        public async Task<DownloadCenterFilesViewModel> BuildModelAsync(DownloadCenterFilesComponentModel componentModel)
        {
            if (componentModel.Files is not null && componentModel.Files.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(componentModel.Files, componentModel.Language, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, componentModel.Token);

                if (files is not null)
                {
                    var filesViewModel = new DownloadCenterFilesViewModel
                    {
                        Id = componentModel.Id,
                        NameLabel = this.globalLocalizer.GetString("Name"),
                        FilenameLabel = this.globalLocalizer.GetString("Filename"),
                        DescriptionLabel = this.globalLocalizer.GetString("Description"),
                        SizeLabel = this.globalLocalizer.GetString("Size"),
                        FilesLabel = this.globalLocalizer.GetString("Files"),
                        DownloadLabel = this.globalLocalizer.GetString("Download"),
                        CopyLinkLabel = this.globalLocalizer.GetString("CopyLink"),
                        CreatedDateLabel = this.globalLocalizer.GetString("CreatedDate"),
                        LastModifiedDateLabel = this.globalLocalizer.GetString("LastModifiedDate"),
                        DownloadSelectedLabel = this.globalLocalizer.GetString("DownloadSelected"),
                        DownloadEverythingLabel = this.globalLocalizer.GetString("DownloadEverything"),
                        SelectFileLabel = this.globalLocalizer.GetString("SelectFile"),
                        DisplayedRowsLabel = this.globalLocalizer.GetString("DisplayedRows"),
                        RowsPerPageLabel = this.globalLocalizer.GetString("RowsPerPage"),
                        SearchLabel = this.globalLocalizer.GetString("Search"),
                        GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                        SearchApiUrl = componentModel.SearchApiUrl,
                        DefaultPageSize = FilesConstants.DefaultPageSize,
                        NoResultsLabel = this.globalLocalizer.GetString("NoResultsLabel")
                    };

                    var fileViewModels = new List<FileViewModel>();

                    foreach (var file in files.Data.OrEmptyIfNull())
                    {
                        var fileViewModel = new FileViewModel
                        {
                            Name = file.Name,
                            Filename = file.Filename,
                            Url = this.mediaService.GetNonCdnMediaUrl(file.Id),
                            Description = file.Description ?? "-",
                            IsProtected = file.IsProtected,
                            Size = this.mediaService.ConvertToMB(file.Size),
                            LastModifiedDate = file.LastModifiedDate,
                            CreatedDate = file.CreatedDate
                        };

                        fileViewModels.Add(fileViewModel);
                    }

                    filesViewModel.Files = new PagedResults<IEnumerable<FileViewModel>>(fileViewModels.Count(), FilesConstants.DefaultPageSize)
                    {
                        Data = fileViewModels
                    };

                    return filesViewModel;
                }
            }

            return default;
        }
    }
}
