using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Files
{
    public class FilesModelBuilder : IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel>
    {
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptions<AppSettings> options;

        public FilesModelBuilder(
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaHelperService mediaHelperService,
            IOptions<AppSettings> options)
        {
            this.mediaRepository = mediaRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaHelperService = mediaHelperService;
            this.options = options;
        }

        public async Task<FilesViewModel> BuildModelAsync(FilesComponentModel componentModel)
        {
            if (componentModel.Files != null)
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
                        DownloadFilesLabel = this.globalLocalizer.GetString("FilesToDownload"),
                        DownloadLabel = this.globalLocalizer.GetString("Download"),
                        CopyLinkLabel = this.globalLocalizer.GetString("CopyLink"),
                        CreatedDateLabel = this.globalLocalizer.GetString("CreatedDate"),
                        LastModifiedDateLabel = this.globalLocalizer.GetString("LastModifiedDate")
                    };

                    var fileViewModels = new List<FileViewModel>();

                    foreach (var file in files.Data.OrEmptyIfNull())
                    {
                        var fileViewModel = new FileViewModel
                        {
                            Name = file.Name,
                            Filename = file.Filename,
                            Url = this.mediaHelperService.GetFileUrl(this.options.Value.MediaUrl, file.Id),
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
