using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.ApiRequestModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ApiControllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterApiController : BaseApiController
    {
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterApiController(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            IDownloadCenterRepository downloadCenterRepository)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.downloadCenterRepository = downloadCenterRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var downloadCenterItems = await this.downloadCenterRepository.GetDownloadCenterItemsAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(DownloadCenterItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, downloadCenterItems);
        }

        [HttpPost]
        [RequestSizeLimit(ApiConstants.Request.RequestSizeLimit)]
        public async Task<IActionResult> Post([FromBody] DownloadCenterItemRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.downloadCenterRepository.SaveAsync(token, language, model.Id, model.CategoriesIds, model.Files);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("DownloadCenterItemSavedSuccessfully").Value });
        }

        //[HttpPost]
        //[RequestSizeLimit(ApiConstants.Request.RequestChunkSizeLimit)]
        //public async Task<IActionResult> PostChunk([FromForm] IFormFile chunk, int? chunkNumber)
        //{
        //    if ((chunk is null) && chunkNumber.HasValue is false)
        //    {
        //        return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        //    }

        //    if (chunk is not null)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            await chunk.CopyToAsync(ms);

        //            await this.filesRepository.SaveChunkAsync(
        //                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
        //                CultureInfo.CurrentUICulture.Name,
        //                ms.ToArray(),
        //                chunk.FileName,
        //                chunkNumber);

        //            return this.StatusCode((int)HttpStatusCode.OK);
        //        }
        //    }

        //    return this.StatusCode((int)HttpStatusCode.OK);
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostChunksComplete(string filename)
        //{
        //    if (string.IsNullOrWhiteSpace(filename) is true)
        //    {
        //        return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        //    }

        //    var fileId = await this.filesRepository.SaveChunksCompleteAsync(
        //        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
        //        CultureInfo.CurrentUICulture.Name,
        //        filename);

        //    var mediaItem = await this.mediaItemsRepository.GetMediaItemAsync(
        //                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
        //                CultureInfo.CurrentUICulture.Name,
        //                fileId);

        //    if (mediaItem is not null)
        //    {
        //        return this.StatusCode(
        //            (int)HttpStatusCode.OK,
        //            new
        //            {
        //                Id = mediaItem.Id,
        //                Url = this.mediaService.GetMediaUrl(mediaItem.Id),
        //                Name = mediaItem.Name,
        //                MimeType = mediaItem.MimeType,
        //                Filename = mediaItem.Filename,
        //                Extension = mediaItem.Extension
        //            });
        //    }

        //    return this.StatusCode((int)HttpStatusCode.BadRequest);
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.downloadCenterRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("DownloadCenterItemDeletedSuccessfully").Value });
        }
    }
}
