using Buyer.Web.Areas.DownloadCenter.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.ApiControllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterApiController : BaseApiController
    {
        private readonly IDownloadCenterRepository downloadCenterRepository;

        public DownloadCenterApiController(
            IDownloadCenterRepository downloadCenterRepository)
        {
            this.downloadCenterRepository = downloadCenterRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.downloadCenterRepository.GetAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }
    }
}
