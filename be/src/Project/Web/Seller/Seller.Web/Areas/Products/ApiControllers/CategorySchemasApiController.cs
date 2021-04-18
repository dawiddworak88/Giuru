using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class CategorySchemasApiController : BaseApiController
    {
        private readonly ICategoriesRepository categoriesRepository;

        public CategorySchemasApiController(ICategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId)
        {
            var categorySchema = await this.categoriesRepository.GetCategorySchemaAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                categoryId);

            return this.StatusCode((int)HttpStatusCode.OK, categorySchema);
        }
    }
}
