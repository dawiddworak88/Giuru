﻿using Buyer.Web.Areas.Products.Repositories.Files;
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
    public class FilesModelBuilder : IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel>
    {
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public FilesModelBuilder(
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IMediaService mediaHelperService)
        {
            this.mediaRepository = mediaRepository;
            this.globalLocalizer = globalLocalizer;
            this.mediaService = mediaHelperService;
        }

        public async Task<FilesViewModel> BuildModelAsync(FilesComponentModel componentModel)
        {
            if (componentModel.Files is not null && componentModel.Files.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(componentModel.Files, componentModel.Language, PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize, componentModel.Token);

                if (files is not null)
                {
                    var filesViewModel = new FilesViewModel
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
                        DisplayedRowsLabel = this.globalLocalizer.GetString("DisplayedRows"),
                        RowsPerPageLabel = this.globalLocalizer.GetString("RowsPerPage"),
                        DefaultPageSize = FilesConstants.DefaultPageSize,
                        GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                        SearchApiUrl = componentModel.SearchApiUrl
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

                    filesViewModel.Files = new PagedResults<IEnumerable<FileViewModel>>(fileViewModels.Count, FilesConstants.DefaultPageSize)
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
