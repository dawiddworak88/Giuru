using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Download.ApiRequestModels;
using Seller.Web.Areas.Download.Repositories.Categories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.ApiControllers
{
    [Area("Download")]
    public class DownloadsApiController : BaseApiController
    {
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;

        public DownloadsApiController(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            ICategoriesRepository categoriesRepository)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            //var categories = await this.categoriesRepository.GetCategoriesAsync(
                //token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(DomainModels.Download.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DownloadRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            //var category = await this.categoriesRepository.SaveAsync(
                //token, language, model.Id, model.Name, model.ParentCategoryId);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("CategorySavedSuccessfully").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            //await this.categoriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
