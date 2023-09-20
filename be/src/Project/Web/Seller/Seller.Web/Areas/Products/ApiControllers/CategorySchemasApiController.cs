using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class CategorySchemasApiController : BaseApiController
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategorySchemasApiController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? categoryId)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categorySchema = await _categoriesRepository.GetCategorySchemasAsync(token, language, categoryId);

            if (categorySchema is not null && categorySchema.Schemas.OrEmptyIfNull().Any())
            {
                var responseModel = new CategorySchemaApiResponseModel
                {
                    Schema = categorySchema.Schemas.FirstOrDefault(x => x.Language == language)?.Schema ?? categorySchema.Schemas.FirstOrDefault()?.Schema,
                    UiSchema = categorySchema.Schemas.FirstOrDefault(x => x.Language == language)?.UiSchema ?? categorySchema.Schemas.FirstOrDefault()?.UiSchema,
                };

                return StatusCode((int)HttpStatusCode.OK, responseModel);
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
