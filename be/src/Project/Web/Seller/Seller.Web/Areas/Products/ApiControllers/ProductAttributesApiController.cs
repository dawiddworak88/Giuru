using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.Services;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductAttributesApiController : BaseApiController
    {
        private readonly IProductAttributesRepository _productAttributesRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IProductAttributesService _productAttributesService;

        public ProductAttributesApiController(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            ICategoriesRepository categoriesRepository,
            IProductAttributesService productAttributesService)
        {
            _productAttributesRepository = productAttributesRepository;
            _productLocalizer = productLocalizer;
            _categoriesRepository = categoriesRepository;
            _productAttributesService = productAttributesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var productAttributes = await _productAttributesRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(ProductAttribute.CreatedDate)} desc");

            return StatusCode((int)HttpStatusCode.OK, productAttributes);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveProductAttributeRequestModel model)
        {
            var productAttributeId = await _productAttributesRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Name);

            return StatusCode((int)HttpStatusCode.OK, new { Id = productAttributeId, Message = _productLocalizer.GetString("ProductAttributeSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _productAttributesService.DeleteAsync(
                token,
                language,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("ProductAttributeDeletedSuccessfully").Value });
        }
    }
}
