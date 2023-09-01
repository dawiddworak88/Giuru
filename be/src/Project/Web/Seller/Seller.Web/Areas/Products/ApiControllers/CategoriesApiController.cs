using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class CategoriesApiController : BaseApiController
    {
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IStringLocalizer productLocalizer;

        public CategoriesApiController(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.categoriesRepository = categoriesRepository;
            this.productLocalizer = productLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var categories = await this.categoriesRepository.GetCategoriesAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Category.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveCategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categoryId = await this.categoriesRepository.SaveAsync(
                token, language, model.Id, model.ParentCategoryId, model.Name, model.Files.Select(x => x.Id.Value), model.Schema, model.UiSchema, model.Order);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = this.productLocalizer.GetString("CategorySavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await this.categoriesRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.productLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
