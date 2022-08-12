using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.ComponentModels.Files;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Shared.ModelBuilders.Files
{
    public class FilesModelBuilder : IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel>
    {
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public FilesModelBuilder(
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaService)
        {
            this.mediaRepository = mediaRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaService;
        }

        public async Task<FilesViewModel> BuildModelAsync(FilesComponentModel componentModel)
        {
            if (componentModel.Files is not null && componentModel.Files.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(componentModel.Files, componentModel.Language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token);

                if (files != null)
                {
                    var filesViewModel = new FilesViewModel
                    {
                        NameLabel = this.globalLocalizer.GetString("Name"),
                        FilenameLabel = this.globalLocalizer.GetString("Filename"),
                        DescriptionLabel = this.globalLocalizer.GetString("Description"),
                        SizeLabel = this.globalLocalizer.GetString("Size"),
                        FilesLabel = this.globalLocalizer.GetString("Files"),
                        DownloadLabel = this.globalLocalizer.GetString("Download"),
                        CopyLinkLabel = this.globalLocalizer.GetString("CopyLink"),
                        CreatedDateLabel = this.globalLocalizer.GetString("CreatedDate"),
                        LastModifiedDateLabel = this.globalLocalizer.GetString("LastModifiedDate"),
                        DisplayedRowsLabel = this.globalLocalizer.GetString("DisplayedRows"),
                        RowsPerPageLabel = this.globalLocalizer.GetString("RowsPerPage")
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
                            Size = string.Format("{0:0.00} MB", file.Size / 1024f / 1024f),
                            LastModifiedDate = file.LastModifiedDate,
                            CreatedDate = file.CreatedDate
                        };

                        fileViewModels.Add(fileViewModel);
                    }

                    filesViewModel.Files = fileViewModels;

                    return filesViewModel;
                }
            }

            return default;
        }
    }
}
