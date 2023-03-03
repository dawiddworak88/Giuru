using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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

        public ProductCardApiController(
            IProductAttributeItemsRepository productAttributeItemsRepository, 
            IProductAttributesRepository productAttributesRepository)
        {
            _productAttributeItemsRepository = productAttributeItemsRepository;
            _productAttributesRepository = productAttributesRepository;
        }

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
