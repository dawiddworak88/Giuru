using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Buyer.Web.Areas.DownloadCenter.Repositories;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.DomainModels.Media;
using Buyer.Web.Shared.Repositories.Media;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ApiControllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterApiController : BaseApiController
    {
        private readonly IDownloadCenterRepository downloadCenterRepository;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IMediaService mediaService;

        public DownloadCenterApiController(
            IDownloadCenterRepository downloadCenterRepository,
            IMediaItemsRepository mediaRepository,
            IMediaService mediaService)
        {
            this.downloadCenterRepository = downloadCenterRepository;
            this.mediaRepository = mediaRepository;
            this.mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.downloadCenterRepository.GetAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var downloadCenterFiles = await this.downloadCenterRepository.GetCategoryFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(DownloadCenterFile.CreatedDate)} desc");

            var filesModel = new List<FileItem>();
            var filesIds = downloadCenterFiles.Data.Select(x => x.Id);

            if (downloadCenterFiles is not null && filesIds.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(token, language, filesIds, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize);

                foreach (var file in files.OrEmptyIfNull())
                {
                    var fileModel = new FileItem
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Filename = file.Filename,
                        Url = this.mediaService.GetNonCdnMediaUrl(file.Id),
                        Description = file.Description ?? "-",
                        IsProtected = file.IsProtected,
                        Size = this.mediaService.ConvertToMB(file.Size),
                        LastModifiedDate = file.LastModifiedDate,
                        CreatedDate = file.CreatedDate
                    };

                    filesModel.Add(fileModel);
                }
            }

            var pagedFiles = new PagedResults<IEnumerable<FileItem>>(filesModel.Count, FilesConstants.DefaultPageSize)
            {
                Data = filesModel
            };

            return this.StatusCode((int)HttpStatusCode.OK, pagedFiles);
        }
    }
}
