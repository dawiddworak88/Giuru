using Buyer.Web.Areas.Download.Repositories;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Download.ApiControllers
{
    [Area("Download")]
    public class DownloadsApiController : BaseApiController
    {
        private readonly IDownloadsRepository downloadsRepository;

        public DownloadsApiController(
            IDownloadsRepository downloadsRepository)
        {
            this.downloadsRepository = downloadsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.downloadsRepository.GetAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }
    }
}
