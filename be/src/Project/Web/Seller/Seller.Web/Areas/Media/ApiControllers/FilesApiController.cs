﻿using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Media.Services.MediaServices;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Media.Repositories.Files;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Shared.Repositories.Media;
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
        private readonly IMediaService mediaService;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IMediaRepository mediaRepository;
        private readonly IStringLocalizer mediaResources;

        public FilesApiController(
            IFilesRepository filesRepository,
            IMediaService mediaService,
            IMediaItemsRepository mediaItemsRepository,
            IMediaRepository mediaRepository,
            IStringLocalizer<MediaResources> mediaResources)
        {
            this.filesRepository = filesRepository;
            this.mediaService = mediaService;
            this.mediaRepository = mediaRepository;
            this.mediaResources = mediaResources;
            this.mediaItemsRepository = mediaItemsRepository;
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

                    if (mediaItem != null)
                    {
                        return this.StatusCode(
                            (int)HttpStatusCode.OK,
                            new
                            {
                                Id = mediaItem.Id,
                                Url = this.mediaService.GetMediaUrl(mediaItem.Id),
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
                                Url = this.mediaService.GetMediaUrl(mediaItem.Id),
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
