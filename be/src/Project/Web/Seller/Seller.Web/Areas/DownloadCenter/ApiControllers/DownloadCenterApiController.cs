using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
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

            var downloadCenterItems = await this.downloadCenterRepository.GetDownloadCenterAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(DownloadCenterItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, downloadCenterItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DownloadCenterItemRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var downloadCenterItemId = await this.downloadCenterRepository.SaveAsync(token, language, model.Id, model.CategoryId, model.Order);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("DownloadCenterItemSavedSuccessfully").Value, Id = downloadCenterItemId });

        }

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
