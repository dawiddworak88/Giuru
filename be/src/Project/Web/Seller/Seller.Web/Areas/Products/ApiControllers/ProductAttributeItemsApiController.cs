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
        private readonly IProductAttributeItemsRepository productAttributeItemsRepository;
        private readonly IStringLocalizer productLocalizer;

        public ProductAttributeItemsApiController(
            IProductAttributeItemsRepository productAttributeItemsRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.productAttributeItemsRepository = productAttributeItemsRepository;
            this.productLocalizer = productLocalizer;
        }

        [HttpGet]
        [Route("[area]/[controller]/{productAttributeId}")]
        public async Task<IActionResult> Get(Guid? productAttributeId, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var productAttributeItems = await this.productAttributeItemsRepository.GetAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                productAttributeId,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(ProductAttributeItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, productAttributeItems);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveProductAttributeItemRequestModel model)
        {
            var productAttributeId = await this.productAttributeItemsRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.ProductAttributeId,
                model.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = productAttributeId, Message = this.productLocalizer.GetString("ProductAttributeItemSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await this.productAttributeItemsRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.productLocalizer.GetString("ProductAttributeItemDeletedSuccessfully").Value });
        }
    }
}
