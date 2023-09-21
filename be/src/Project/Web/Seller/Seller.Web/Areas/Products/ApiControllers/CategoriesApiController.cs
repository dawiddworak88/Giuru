using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
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
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IStringLocalizer _productLocalizer;

        public CategoriesApiController(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _categoriesRepository = categoriesRepository;
            _productLocalizer = productLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var categories = await _categoriesRepository.GetCategoriesAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Category.Order)} asc");

            return StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveCategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categoryId = await _categoriesRepository.SaveAsync(
                token, 
                language, 
                model.Id, 
                model.ParentCategoryId, 
                model.Name, 
                model.Files.Select(x => x.Id.Value),
                model.Schemas.OrEmptyIfNull().Select(x => new CategorySchema
                {
                    Id = x.Id,
                    Schema = x.Schema,
                    UiSchema = x.UiSchema,
                    Language = x.Language
                }), 
                model.Order);

            return StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = _productLocalizer.GetString("CategorySavedSuccessfully").Value });
        }

        [HttpPost]        
        public async Task<IActionResult> Order([FromBody] SaveCategoryOrderRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentCulture.Name;

            var category = await _categoriesRepository.GetCategoryAsync(token, language, model.Id);

            if(category is not null) 
            {
                var categorySchemas = await _categoriesRepository.GetCategorySchemasAsync(token, language, model.Id);

                if (categorySchemas is not null && categorySchemas.Schemas.OrEmptyIfNull().Any())
                {
                    await _categoriesRepository.SaveAsync(
                        token, language, model.Id, category.ParentId, category.Name, category.Files, 
                        categorySchemas.Schemas.Select(x => new CategorySchema
                        {
                            Id = x.Id,
                            Schema = x.Schema,
                            UiSchema = x.UiSchema,
                            Language = x.Language
                        }), category.Order);

                    return StatusCode((int)HttpStatusCode.OK);
                }
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await _categoriesRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
