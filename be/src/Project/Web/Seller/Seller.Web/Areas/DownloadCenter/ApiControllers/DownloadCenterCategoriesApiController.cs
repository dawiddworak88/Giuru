using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.ApiRequestModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenterCategories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ApiControllers
{
    [Area("DownloadCenter")]
    public class DownloadCenterCategoriesApiController : BaseApiController
    {
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository;

        public DownloadCenterCategoriesApiController(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            IDownloadCenterCategoriesRepository downloadCenterCategoriesRepository)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.downloadCenterCategoriesRepository = downloadCenterCategoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.downloadCenterCategoriesRepository.GetCategoriesAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(DownloadCenterCategory.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DownloadCenterCategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categoryId = await this.downloadCenterCategoriesRepository.SaveAsync(token, language, model.Id, model.Name, model.ParentCategoryId, model.IsVisible);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = this.downloadCenterLocalizer.GetString("CategorySavedSuccessfully").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.downloadCenterCategoriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
