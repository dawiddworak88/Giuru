using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.ApiRequestModels;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Areas.News.Repositories.Categories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ApiControllers
{
    [Area("News")]
    public class CategoriesApiController : BaseApiController
    {
        private readonly IStringLocalizer newsLocalizer;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesApiController(
            IStringLocalizer<NewsResources> newsLocalizer,
            ICategoriesRepository categoriesRepository)
        {
            this.newsLocalizer = newsLocalizer;
            this.categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.categoriesRepository.GetCategoriesAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Category.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var category = await this.categoriesRepository.SaveAsync(
                token, language, model.Id, model.Name, model.ParentCategoryId);

            return this.StatusCode((int)HttpStatusCode.OK, new { category, Message = this.newsLocalizer.GetString("CategorySavedSuccessfully").Value });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.categoriesRepository.DeleteAsync(token, language, id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.newsLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
