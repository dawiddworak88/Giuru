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
    public class ProductAttributesApiController : BaseApiController
    {
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IStringLocalizer productLocalizer;

        public ProductAttributesApiController(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.productAttributesRepository = productAttributesRepository;
            this.productLocalizer = productLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var productAttributes = await this.productAttributesRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(ProductAttribute.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, productAttributes);
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
