using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.Repositories;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Services.ContentDeliveryNetworks;
using System;
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
        private readonly IMediaRepository mediaRepository;
        private readonly IStringLocalizer mediaResources;
        private readonly ICdnService cdnService;

        public FilesApiController(
            IFilesRepository filesRepository,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            IMediaItemsRepository mediaItemsRepository,
            IMediaRepository mediaRepository,
            IStringLocalizer<MediaResources> mediaResources,
            ICdnService cdnService)
        {
            this.filesRepository = filesRepository;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
            this.mediaRepository = mediaRepository;
            this.mediaResources = mediaResources;
            this.mediaItemsRepository = mediaItemsRepository;
            this.cdnService = cdnService;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] IFormFile file, List<IFormFile> files, string id)
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
                        file.FileName, id);

                    var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(
                        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                        CultureInfo.CurrentUICulture.Name,
                        fileId);

                    Console.WriteLine(fileId);

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
                            fileItem.FileName, id);

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
                                Url = this.cdnService.GetCdnUrl(this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, mediaItem.Id, true)),
                                Name = mediaItem.Name,
                                MimeType = mediaItem.MimeType,
                                Filename = mediaItem.Filename,
                                Extension = mediaItem.Extension
                            }));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.mediaRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.mediaResources.GetString("MediaDeletedSuccessfully").Value });
        }
    }
}
