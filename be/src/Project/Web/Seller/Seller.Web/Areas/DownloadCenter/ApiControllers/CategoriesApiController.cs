using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.DownloadCenter.ApiRequestModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.DownloadCenter.Repositories.Categories;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.ApiControllers
{
    [Area("DownloadCenter")]
    public class CategoriesApiController : BaseApiController
    {
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesApiController(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            ICategoriesRepository categoriesRepository)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.categoriesRepository.GetCategoriesAsync(token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Category.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var category = await this.categoriesRepository.SaveAsync(token, language, model.Id, model.Name, model.ParentCategoryId, model.IsVisible, model.Files.Select(x => x.Id.Value));

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = category, Message = this.downloadCenterLocalizer.GetString("CategorySavedSuccessfully").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.categoriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.downloadCenterLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
