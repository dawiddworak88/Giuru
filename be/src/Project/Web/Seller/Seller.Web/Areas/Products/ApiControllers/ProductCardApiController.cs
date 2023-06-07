using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.Definitions;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductCardApiController : BaseApiController
    {
        private readonly IProductAttributeItemsRepository _productAttributeItemsRepository;
        private readonly IProductAttributesRepository _productAttributesRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public ProductCardApiController(
            IProductAttributeItemsRepository productAttributeItemsRepository, 
            IProductAttributesRepository productAttributesRepository,
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _productAttributeItemsRepository = productAttributeItemsRepository;
            _productAttributesRepository = productAttributesRepository;
            _categoriesRepository = categoriesRepository;
            _productLocalizer = productLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ProductCardRequestModel request)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _categoriesRepository.SaveAsync(token, language, request.Id, request.Schema, request.UiSchema);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("SuccessfullySavedProductCard").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Definition(Guid? id) {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var productAttribute = await _productAttributesRepository.GetByIdAsync(token, language, id);

            if (productAttribute is not null)
            {
                var response = new ProductCardDefinitionResponseModel
                {
                    Title = productAttribute.Name,
                    Type = ProductsConstants.ProductCardDefinitions.DefaultDefinitionType
                };

                var productAttributeItems = await _productAttributeItemsRepository.GetAsync(token, language, id);

                if (productAttributeItems is not null)
                {
                    response.AnyOf = productAttributeItems.OrEmptyIfNull().Select(x => new ProductCardDefinitionItemResponseModel
                    {
                        Type = ProductsConstants.ProductCardDefinitions.DefaultDefinitionType,
                        Enum = new List<Guid>
                        {
                            x.Id
                        },
                        Title = x.Name
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, new { Data = response });
                }
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
