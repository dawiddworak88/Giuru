using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.Repositories;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ApiControllers
{
    [Area("Media")]
    public class FilesApiController : BaseApiController
    {
        private readonly IFilesRepository filesRepository;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;
        private readonly IMediaItemsRepository mediaItemsRepository;

        public FilesApiController(
            IFilesRepository filesRepository,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            IMediaItemsRepository mediaItemsRepository)
        {
            this.filesRepository = filesRepository;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.mediaItemsRepository = mediaItemsRepository;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] IFormFile file, List<IFormFile> files)
        {
            if (file == null && (files == null || !files.Any()))
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    var fileId = await this.filesRepository.SaveAsync(
                        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                        CultureInfo.CurrentUICulture.Name,
                        ms.ToArray(),
                        file.FileName);

                    var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(
                        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                        CultureInfo.CurrentUICulture.Name,
                        fileId);

                    if (mediaItem != null)
                    {
                        return this.StatusCode(
                            (int)HttpStatusCode.OK,
                            new
                            {
                                Id = mediaItem.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, mediaItem.Id, true),
                                Name = mediaItem.Name,
                                MimeType = mediaItem.MimeType,
                                Filename = mediaItem.Filename,
                                Extension = mediaItem.Extension
                            });
                    }

                    return this.StatusCode((int)HttpStatusCode.BadRequest);
                }
            }
            else
            {
                var media = new List<MediaItem>();

                foreach (var fileItem in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        await fileItem.CopyToAsync(ms);

                        var fileId = await this.filesRepository.SaveAsync(
                            await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                            CultureInfo.CurrentUICulture.Name,
                            ms.ToArray(),
                            fileItem.FileName);

                        var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(
                            await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                            CultureInfo.CurrentUICulture.Name,
                            fileId);

                        if (mediaItem != null)
                        {
                            media.Add(mediaItem);
                        }
                    }
                }

                return this.StatusCode(
                            (int)HttpStatusCode.OK,
                            media.Select(mediaItem => new
                            {
                                Id = mediaItem.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, mediaItem.Id, true),
                                Name = mediaItem.Name,
                                MimeType = mediaItem.MimeType,
                                Filename = mediaItem.Filename,
                                Extension = mediaItem.Extension
                            }));
            }
        }
    }
}
