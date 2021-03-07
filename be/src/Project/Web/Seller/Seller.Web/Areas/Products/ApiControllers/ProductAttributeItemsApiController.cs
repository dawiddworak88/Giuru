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
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductAttributeItemsApiController : BaseApiController
    {
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IStringLocalizer productLocalizer;

        public ProductAttributeItemsApiController(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.productAttributesRepository = productAttributesRepository;
            this.productLocalizer = productLocalizer;
        }

        [HttpGet]
        [Route("api/[area]/[controller]/{productAttributeId}")]
        public async Task<IActionResult> Get(Guid? productAttributeId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var productAttributeItems = await this.productAttributesRepository.GetProductAttributeItemsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                productAttributeId,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(ProductAttribute.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, productAttributeItems);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveProductAttributeRequestModel model)
        {
            var productAttributeId = await this.productAttributesRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = productAttributeId, Message = this.productLocalizer.GetString("ProductAttributeSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await this.productAttributesRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.productLocalizer.GetString("ProductAttributeDeletedSuccessfully").Value });
        }
    }
}
