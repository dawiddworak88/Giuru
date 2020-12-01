using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Globalization;
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
                itemsPerPage);

            return this.StatusCode((int)HttpStatusCode.OK, categories);
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
